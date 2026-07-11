using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Services.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        [Header("Inertia")]
        [SerializeField] private bool enableInertia = false;
        [SerializeField] private float inertiaDamping = 8f;   // чем выше — тем быстрее тормозит

        [Header("Zoom")]
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 15f;
        [SerializeField] private float zoomSmoothing = 10f;
        [SerializeField] private float mouseScrollSensitivity = 1.0f;
        [SerializeField] private float pinchZoomSensitivity = 0.02f;

        [Header("Boundaries")]
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minZ = -10f;
        [SerializeField] private float maxZ = 10f;
        [SerializeField] private float groundY = 0f;

        private float _targetZoom;
        private Vector2 _clippingPlanes;
        private Vector3 _targetPosition;

        private bool _isTouchDragging;
        private int _dragFingerId = -1;
        private Vector3 _touchAnchorWorld;

        private bool _isMouseDragging;
        private Vector3 _mouseAnchorWorld;

        // Inertia
        private Vector3 _velocity;          // world units/sec
        private Vector3 _prevTargetPosition;

        private void OnEnable() => EnhancedTouchSupport.Enable();
        private void OnDisable() => EnhancedTouchSupport.Disable();

        private void Awake()
        {
            _targetZoom = _camera.orthographicSize;
            _clippingPlanes = new Vector2(_camera.nearClipPlane, _camera.farClipPlane);
            _targetPosition = transform.position;
            _prevTargetPosition = _targetPosition;
        }

        private void Update()
        {
            bool wasDragging = _isTouchDragging || _isMouseDragging;

            HandleTouchDrag();
            HandleMouseDrag();
            HandleMouseScroll();
            HandlePinchZoom();

            bool isDragging = _isTouchDragging || _isMouseDragging;

            if (enableInertia)
            {
                if (isDragging)
                {
                    // Считаем velocity по смещению _targetPosition за кадр
                    _velocity = (_targetPosition - _prevTargetPosition) / Time.deltaTime;
                }
                else if (wasDragging)
                {
                    // Только что отпустили — velocity уже посчитан, начинаем скольжение
                }
                else
                {
                    // Скользим с затуханием
                    _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.deltaTime * inertiaDamping);
                    Vector3 inertiaPos = _targetPosition + _velocity * Time.deltaTime;
                    inertiaPos.x = Mathf.Clamp(inertiaPos.x, minX, maxX);
                    inertiaPos.z = Mathf.Clamp(inertiaPos.z, minZ, maxZ);
                    inertiaPos.y = _targetPosition.y;
                    _targetPosition = inertiaPos;
                }
            }

            _prevTargetPosition = _targetPosition;

            transform.position = _targetPosition;
            _camera.orthographicSize =
                Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * zoomSmoothing);
            RefreshClippingPlanes(transform.position.y);
        }

        private void HandleTouchDrag()
        {
            var touches = Touch.activeTouches;

            if (touches.Count >= 2)
            {
                _isTouchDragging = false;
                _dragFingerId = -1;
                return;
            }

            if (touches.Count == 0)
            {
                _isTouchDragging = false;
                _dragFingerId = -1;
                return;
            }

            Touch t = touches[0];

            if (t.phase == TouchPhase.Began)
            {
                if (IsPointerOverUI(t.finger.index)) return;
                _isTouchDragging = true;
                _dragFingerId = t.finger.index;
                _touchAnchorWorld = ScreenToGroundPoint(t.screenPosition);
                return;
            }

            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                _isTouchDragging = false;
                _dragFingerId = -1;
                return;
            }

            if (!_isTouchDragging || t.finger.index != _dragFingerId) return;

            // Стабильная формула: anchorWorld всегда остаётся под пальцем
            Vector3 currentWorld = ScreenToGroundPoint(t.screenPosition);
            ApplyDrag(_touchAnchorWorld, currentWorld);
        }

        private void HandleMouseDrag()
        {
            if (Touch.activeTouches.Count > 0)
            {
                _isMouseDragging = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUI()) return;
                _isMouseDragging = true;
                _mouseAnchorWorld = ScreenToGroundPoint(Input.mousePosition);
                return;
            }

            if (!Input.GetMouseButton(0))
            {
                _isMouseDragging = false;
                return;
            }

            if (!_isMouseDragging) return;

            Vector3 currentWorld = ScreenToGroundPoint(Input.mousePosition);
            ApplyDrag(_mouseAnchorWorld, currentWorld);
        }

        // anchorWorld - currentWorld + camera.pos = позиция где anchor окажется под пальцем
        private void ApplyDrag(Vector3 anchorWorld, Vector3 currentWorld)
        {
            Vector3 newPos = anchorWorld - currentWorld + transform.position;
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);
            newPos.y = transform.position.y;
            _targetPosition = newPos;
        }

        private void HandleMouseScroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0f) return;
            _targetZoom -= scroll * mouseScrollSensitivity;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
        }

        private void HandlePinchZoom()
        {
            var touches = Touch.activeTouches;
            if (touches.Count != 2) return;

            var t0 = touches[0];
            var t1 = touches[1];
            Vector2 prev0 = t0.screenPosition - t0.delta;
            Vector2 prev1 = t1.screenPosition - t1.delta;

            float diff = (t0.screenPosition - t1.screenPosition).magnitude - (prev0 - prev1).magnitude;
            _targetZoom -= diff * pinchZoomSensitivity;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
        }

        private Vector3 ScreenToGroundPoint(Vector2 screenPos)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0f));
            if (Mathf.Abs(ray.direction.y) < 0.0001f) return ray.origin;
            float t = (groundY - ray.origin.y) / ray.direction.y;
            return ray.GetPoint(t);
        }

        private bool IsPointerOverUI(int fingerId = -1) =>
            EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);

        private void RefreshClippingPlanes(float y)
        {
            _camera.nearClipPlane = _clippingPlanes.x + y;
            _camera.farClipPlane = _clippingPlanes.y + y;
        }
    }
}
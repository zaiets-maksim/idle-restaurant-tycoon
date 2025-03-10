using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private float moveSpeed = 0.1f;
        [SerializeField] private float dragSpeed = 0.01f;

        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 15f;
        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private float mouseScrollSensitivity = 1.0f;
        
        [SerializeField] private float _sensitivity = 0.5f;

        [Header("Boundaries")] 
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minZ = -10f;
        [SerializeField] private float maxZ = 10f;

        private Vector3 dragOrigin;
        private float targetZoom;
        private Vector3 _lastMousePosition;
        private Vector2 _clippingPlanes;
        private bool _canMove = true;

        private void Awake()
        {
            targetZoom = _camera.orthographicSize;
            _clippingPlanes = new Vector2(_camera.nearClipPlane, _camera.farClipPlane);
        }

        private void CheckMovementInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
                _canMove = !IsMouseOverUI();
            }

            if (!Input.GetMouseButton(0) || !_canMove)
                return;

            Vector3 mouseWorldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lastWorldPoint = _camera.ScreenToWorldPoint(_lastMousePosition);
            Vector3 delta = mouseWorldPoint - lastWorldPoint;
            _lastMousePosition = Input.mousePosition;
            _camera.transform.position -= delta;
        }

        private void Update()
        {
            CheckMovementInput();
            HandleZoom();

            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * 10f);
            float x = Mathf.Clamp(transform.position.x, minX, maxX);
            float z = Mathf.Clamp(transform.position.z, minZ, maxZ);
            float y = transform.position.y;

            transform.position = new Vector3(x, y, z);

            RefreshClippingPlanes(y);
            // HandleTouch();
            // HandleMouse();
        }

        private void HandleZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                targetZoom -= scrollInput * mouseScrollSensitivity;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
        }

        private bool IsMouseOverUI() =>
            EventSystem.current.IsPointerOverGameObject();

        private void RefreshClippingPlanes(float y)
        {
            _camera.nearClipPlane = _clippingPlanes.x + y;
            _camera.farClipPlane = _clippingPlanes.y + y;
        }

        private void HandleTouch()
        {
            // Mobile touch input handling
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    dragOrigin = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 touchDelta = new Vector3(touch.position.x, touch.position.y, 0) - dragOrigin;
                    Vector3 move = new Vector3(touchDelta.x, touchDelta.y, 0) * dragSpeed;
                    transform.Translate(-move, Space.World);
                    dragOrigin = touch.position;
                }
            }
        }

        private void HandleMouse()
        {
            // Editor handling (mouse input as a fallback for the editor)
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - dragOrigin;
                Vector3 move = new Vector3(mouseDelta.x, 0, mouseDelta.y) * dragSpeed;
                transform.Translate(-move, Space.World);
                dragOrigin = Input.mousePosition;
            }
        }
    }
}
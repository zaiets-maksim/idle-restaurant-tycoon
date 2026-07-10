using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        [Header("Drag")]
        [SerializeField] private float dragSmoothing = 8f;
        [SerializeField] private float dragSensitivity = 1f;

        [Header("Zoom")]
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 15f;
        [SerializeField] private float zoomSmoothing = 10f;
        [SerializeField] private float mouseScrollSensitivity = 1.0f;

        [Header("Boundaries")] 
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minZ = -10f;
        [SerializeField] private float maxZ = 10f;

        private float targetZoom;
        private Vector3 _lastMousePosition;
        private Vector2 _clippingPlanes;
        private bool _canMove = true;
        private Vector3 _targetPosition;

        private void Awake()
        {
            targetZoom = _camera.orthographicSize;
            _clippingPlanes = new Vector2(_camera.nearClipPlane, _camera.farClipPlane);
            _targetPosition = transform.position;
        }

        private void Update()
        {
            CheckDragInput();
            HandleZoom();

            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * dragSmoothing);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothing);

            RefreshClippingPlanes(transform.position.y);
        }

        private void CheckDragInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
                _canMove = !IsMouseOverUI();
            }

            if (!Input.GetMouseButton(0) || !_canMove)
                return;

            Vector3 screenDelta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            screenDelta.y = -screenDelta.y;

            Vector3 worldDelta = _camera.cameraToWorldMatrix.MultiplyVector(screenDelta) * dragSensitivity;

            Vector3 newPosition = _targetPosition + worldDelta;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
            newPosition.y = _targetPosition.y;

            _targetPosition = newPosition;
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
    }
}
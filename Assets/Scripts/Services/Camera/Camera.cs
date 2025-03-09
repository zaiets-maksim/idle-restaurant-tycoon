using UnityEngine;

public class CemeraTouch : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField] private float mouseMoveSensitivity = 1.0f;
    [SerializeField] private float inertiaFactor = 0.92f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float mouseScrollSensitivity = 1.0f;

    [Header("Boundaries")] [SerializeField]
    private float minX = -10f;

    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minZ = -10f;
    [SerializeField] private float maxZ = 10f;

    private Vector3 touchStart;
    private Vector3 mouseStart;
    private Vector3 velocity;
    private Camera cam;
    private float targetZoom;
    private bool isDragging = false;

    private void Awake()
    {
        targetZoom = _camera.orthographicSize;
    }

    private void Update()
    {
        // HandleTouch();
        HandleMouse();
        
        transform.position += velocity;
        velocity *= inertiaFactor;

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * 10f);

        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float z = Mathf.Clamp(transform.position.z, minZ, maxZ);
        float y = transform.position.y;

        transform.position = new Vector3(x, y, z);
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                velocity = Vector3.zero;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 direction = touch.position - new Vector2(touchStart.x, touchStart.y);

                // velocity = new Vector3(-direction.x, 0, -direction.y) * moveSpeed * Time.deltaTime;
                velocity = Vector3.Lerp(velocity, new Vector3(-direction.x, 0, -direction.y) * (moveSpeed * Time.deltaTime), 0.1f);
                Debug.Log(velocity);
                touchStart = touch.position;
            }
            
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                targetZoom += deltaMagnitudeDiff * zoomSpeed * 0.01f;

                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
        }
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStart = Input.mousePosition;
            isDragging = true;
            velocity = Vector3.zero;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 direction = mousePos - mouseStart;

            velocity = Vector3.Lerp(velocity, new Vector3(-direction.x, 0, -direction.y) * (mouseMoveSensitivity * Time.deltaTime), 0.1f);
            mouseStart = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetZoom -= scrollInput * mouseScrollSensitivity;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }
}
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float Angle { get; set; }
    public Transform Target { get; set; }

    [SerializeField]
    private float _rotationSpeed = 3;
    [SerializeField]
    private float _zoomSpeed = 5;
    [SerializeField]
    private float _smoothFollow = 0.5f;

    private float _maxZoom = 10;
    private float _minZoom = 2;

    private Vector3 _velocity = Vector3.zero;
    private Transform _cam;
    public Transform CameraTransform
    {
        get
        {
            if (!_cam) _cam = Camera.main.transform;
            return _cam;
        }
    }

    public CameraMovement SetLookTarget(Vector3 lookAt)
    {
        CameraTransform.LookAt(lookAt);

        return this;
    }

    private void Update()
    {
        if (!Target) return;

        float xMovement = Input.GetAxis("Mouse X");
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float stickScroll = Input.GetAxis("RightVertical");

        if (xMovement != 0)
        {
            Angle += xMovement * _rotationSpeed;
            transform.rotation = Quaternion.Euler(new Vector3(0, Angle, 0));
        }
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, _smoothFollow);

        if (scroll != 0)
        {
            SetCameraZoom(scroll);
        }
        else if (stickScroll != 0)
        {
            stickScroll = -stickScroll * Time.deltaTime;
            SetCameraZoom(stickScroll);
        }
    }

    private void SetCameraZoom(float scrollValue)
    {
        Vector3 newZoom = CameraTransform.position + CameraTransform.forward * scrollValue * _zoomSpeed;

        float distance = Vector3.Distance(Target.position, newZoom);

        if (distance <= _maxZoom && distance >= _minZoom) CameraTransform.position = newZoom;
    }

    private void FixedUpdate()
    {
        if (!Target) return;

        float xMovement = Input.GetAxis("RightHorizontal");

        if (xMovement != 0)
        {
            Angle += xMovement * _rotationSpeed;
            transform.rotation = Quaternion.Euler(new Vector3(0, Angle, 0));
        }
    }
}

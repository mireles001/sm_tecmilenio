using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public float Angle { get; set; }
  public Transform Target { get; set; }

  [SerializeField]
  private float _maxZoom = 20f;
  [SerializeField]
  private float _minZoom = 2f;
  [SerializeField]
  private float _zoomSpeed = 5f;
  [SerializeField]
  private float _smoothFollow = 1f;
  [SerializeField]
  private float _rotationSpeed = 3f;
  private Vector3 _velocity = Vector3.zero;
  private Transform _camera;

  private void Awake()
  {
    _camera = Camera.main.transform;
  }

  public void SetLookTarget(Vector3 lookAt)
  {
    _camera.LookAt(lookAt);
  }

  private void Update()
  {
    if (Target == null)
    {
      return;
    }

    float xMovement = Input.GetAxis("Mouse X");

    if (xMovement != 0)
    {
      Angle += xMovement * _rotationSpeed;
      transform.rotation = Quaternion.Euler(new Vector3(0f, Angle, 0f));
    }

    transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, _smoothFollow);

    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0f)
    {
      _camera.position = _camera.position + _camera.forward * scroll * _zoomSpeed;
    }

    float stickScroll = Input.GetAxis("RightVertical");
    if (stickScroll != 0f)
    {
      stickScroll = -stickScroll * Time.deltaTime;
      Vector3 newZoom = _camera.position + _camera.forward * stickScroll * _zoomSpeed;

      float distance = Vector3.Distance(transform.position, newZoom);

      if (distance <= _maxZoom && distance >= _minZoom)
      {
        _camera.position = newZoom;
      }
    }
  }

  private void FixedUpdate()
  {
    if (Target == null)
    {
      return;
    }

    float xMovement = Input.GetAxis("RightHorizontal");

    if (xMovement != 0)
    {
      Angle += xMovement * _rotationSpeed;
      transform.rotation = Quaternion.Euler(new Vector3(0f, Angle, 0f));
    }
  }
}

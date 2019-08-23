using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  [SerializeField]
  private float _smoothTime = 1f;
  private float _camAngle = 0f;
  [SerializeField]
  private float _zoomSpeed = 5f;
  private float _camZoom = 0f;
  [SerializeField, Range(0, 100)]
  private float _lookAtPercentage = 70f;
  [SerializeField]
  private Vector2 _minMaxZoom = new Vector2(1f, 10f);
  private Vector3 _velocity;
  private Transform _cam;
  private Transform _player;
  private PlayerInput _input;

  private void Start()
  {
    _cam = Camera.main.transform;
    _player = GameObject.FindGameObjectWithTag("Player").transform;
    _input = _player.gameObject.GetComponent<PlayerInput>();
    _camZoom = Vector3.Distance(_cam.position, transform.position);

    Vector3 characterHeight = Vector3.up * (_player.gameObject.GetComponent<CapsuleCollider>().height * (_lookAtPercentage / 100f));

    _cam.LookAt(_player.position + characterHeight);
  }

  private void Update()
  {
    if (_input && _input.LookDirection.x != 0)
    {
      _camAngle -= _input.LookDirection.x * Time.deltaTime * 180f;
      transform.rotation = Quaternion.Euler(0, _camAngle, 0);
    }

    transform.position = Vector3.SmoothDamp(transform.position, _player.position, ref _velocity, _smoothTime);

    if (_input && _input.LookDirection.z != 0)
    {
      Vector3 newZoom = _cam.position + _cam.forward * _input.LookDirection.z * _zoomSpeed * Time.deltaTime;

      if (Vector3.Distance(newZoom, _player.position) > _minMaxZoom.x && Vector3.Distance(newZoom, _player.position) < _minMaxZoom.y)
      {
        _cam.position = newZoom;
      }
    }
  }
}

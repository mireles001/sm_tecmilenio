using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public Transform target;
  [SerializeField]
  private bool _changeHeight = true;
  [SerializeField]
  private float _damp = 1f;
  private float _heightOffset;
  private Vector3 _velocity;

  [SerializeField]
  private bool _autoForward = false;
  private float _currentX;
  [SerializeField]
  private float _autoForwardSpeed = 1;
  private float _deathX;

  private bool _movingCamera = true;
  private bool _finishPositionReached = false;
  private float _duration = 0.5f;
  private float _timer = 0;
  private Vector3 _startFinishPosition;
  private Vector3 _endFinishPosition;

  private void Start()
  {
    if (_autoForward && _autoForwardSpeed <= 0) _autoForward = false;

    _heightOffset = Mathf.Abs(transform.position.y - target.position.y);
    transform.position = GetNewPosition();

    _currentX = transform.position.x;

    if (_autoForward)
    {
      Vector3 death = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0,0,Vector3.Distance(transform.position, target.position)));
      _deathX = -Mathf.Abs(target.position.x - death.x);
    }
  }

  private void Update()
  {
    if (_movingCamera)
    {
      transform.position = Vector3.SmoothDamp(transform.position, GetNewPosition(Time.deltaTime), ref _velocity, _damp);
    }
    else if (!_finishPositionReached)
    {
      _timer += Time.deltaTime;
      transform.position = Vector3.Lerp(_startFinishPosition, _endFinishPosition, _timer/_duration);

      if (_timer >= _duration) _finishPositionReached = true;
    }
  }

  private Vector3 GetNewPosition(float deltaTime = 0)
  {
    Vector3 newPosition = target.position;
    newPosition.z = transform.position.z;

    if (_changeHeight)
    {
      newPosition.y = target.position.y + _heightOffset;
    }
    else
    {
      newPosition.y = transform.position.y;
    }

    if (_autoForward && deltaTime > 0)
    {
      newPosition.x = _currentX += deltaTime * _autoForwardSpeed;

      if (target.position.x < transform.position.x + _deathX) target.gameObject.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
    }

    return newPosition;
  }

  public void FinishGame(Vector3 finishPosition)
  {
    _movingCamera = false;
    _startFinishPosition = transform.position;
    _endFinishPosition = finishPosition;
    _endFinishPosition.y += _heightOffset;
    _endFinishPosition.z = transform.position.z;
  }
}

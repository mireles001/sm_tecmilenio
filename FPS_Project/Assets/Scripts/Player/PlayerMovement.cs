using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField]
  private float _jumpSpeed;
  [SerializeField]
  private float _runSpeed;
  [SerializeField]
  private float camSens = 0.5f;
  [SerializeField]
  private Transform _cameraPos;
  private PlayerCore _core;
  private CharacterController _char;
  private FpvAnimation _fpv;

  private Vector3 _velocity = new Vector3();
  private Vector3 _lastMouse = new Vector3(255, 255, 255);

  public void StartUp()
  {
    _core = GetComponent<PlayerCore>();

    Camera.main.transform.parent = _cameraPos;
    Camera.main.transform.localPosition = Vector3.zero;
    Camera.main.transform.localRotation = Quaternion.identity;
    _char = GetComponent<CharacterController>();
  }

  private void LateUpdate()
  {
    if (!_core.IsLocked)
    {
      _lastMouse = Input.mousePosition - _lastMouse;
      _lastMouse = new Vector3(-_lastMouse.y * camSens, _lastMouse.x * camSens, 0);

      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _lastMouse.y, 0);
      _cameraPos.eulerAngles = new Vector3(_cameraPos.eulerAngles.x + _lastMouse.x, _cameraPos.eulerAngles.y, 0);
      _lastMouse = Input.mousePosition;

      if (_char.isGrounded && Input.GetButtonDown("Jump"))
      {
        _velocity.y = _jumpSpeed;
      }

      _velocity.x = 0;
      _velocity.z = 0;
      _velocity += GetBaseInput() * _runSpeed;

      _velocity += Physics.gravity * Time.deltaTime;

      _char.Move(_velocity * Time.deltaTime);
    }
  }

  private Vector3 GetBaseInput()
  {
    Vector3 moveDirection = new Vector3();
    moveDirection += Input.GetAxis("Vertical") * transform.forward
    + Input.GetAxis("Horizontal") * transform.right;
    moveDirection.y = 0;
    // TODO: Avoid going from 0 to 1. Smooth transition.
    moveDirection.Normalize();
    return moveDirection;
  }

  public void SetFpv(FpvAnimation fpv)
  {
    _fpv = fpv.StartUp(this);
  }

  public FpvAnimation Fpv
  {
    get
    {
      return _fpv;
    }
  }

  public void SetSpeed(float value)
  {
    _runSpeed = value;
  }

  public void SetJump(float value)
  {
    _jumpSpeed = value;
  }

  public Transform CameraPos
  {
    get
    {
      return _cameraPos;
    }
  }
}

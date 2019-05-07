using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField]
  private float _jumpSpeed = 0f;
  [SerializeField]
  private float _runSpeed = 0f;
  [SerializeField]
  private float camSens = 0.5f;
  private Transform _cameraPos;
  private Camera _mainCam;
  private Camera _fpvCam;
  private PlayerCore _core;
  private CharacterController _char;

  private Vector3 _velocity = new Vector3();
  private Vector3 _lastMouse = new Vector3(255, 255, 255);

  private void Awake()
  {
    _core = GetComponent<PlayerCore>();
    _char = GetComponent<CharacterController>();

    _mainCam = Camera.main;
    _fpvCam = Instantiate(_mainCam);
    Destroy(_fpvCam.gameObject.GetComponent<AudioListener>());

    _fpvCam.name = "Fpv Camera";
    _fpvCam.gameObject.tag = "Untagged";
    _fpvCam.depth = 1;
    _fpvCam.clearFlags = CameraClearFlags.Depth;
    _fpvCam.cullingMask = 1 << 9;
  }

  public void StartUp(float pos, Transform character)
  {
    if (!_cameraPos)
    {
      _cameraPos = new GameObject("cameras").transform;
      _cameraPos.SetPositionAndRotation(transform.position, transform.rotation);
      _cameraPos.parent = transform;
      _mainCam.transform.parent = _cameraPos;
      _fpvCam.transform.parent = _cameraPos;
    }

    _cameraPos.localPosition = new Vector3(0f, pos, 0f);
    character.parent = _cameraPos;

    _mainCam.transform.SetPositionAndRotation(_cameraPos.position, _cameraPos.rotation);
    _fpvCam.transform.SetPositionAndRotation(_cameraPos.position, _cameraPos.rotation);

    _core.Fpv.StartUp();
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

  public CharacterController CharRb
  {
    get
    {
      return _char;
    }
  }

  public float JumpSpeed
  {
    get
    {
      return _jumpSpeed;
    }
    set
    {
      _jumpSpeed = value;
    }
  }

  public float RunSpeed
  {
    get
    {
      return _runSpeed;
    }
    set
    {
      _runSpeed = value;
    }
  }
}

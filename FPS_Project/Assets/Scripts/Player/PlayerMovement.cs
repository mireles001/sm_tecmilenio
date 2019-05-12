using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField]
  private float _jumpSpeed = 0f;
  [SerializeField]
  private float _runSpeed = 0f;
  [SerializeField]
  private float _lookSpeed = 5f;
  private float _lookClamp;
  private Vector2 _lookRotation = Vector2.zero;
  private Vector3 _moveDirection;
  private Vector3 _velocity = new Vector3();
  private Transform _innerContainer;
  private Camera _mainCam;
  private Camera _fpvCam;
  private PlayerCore _core;
  private CharacterController _char;

  private void Awake()
  {
    _core = GetComponent<PlayerCore>();
    _char = GetComponent<CharacterController>();
    _mainCam = Camera.main;
    _fpvCam = CreateFpvCamera(_mainCam);
    _lookClamp = 80f / _lookSpeed;
  }

  public void StartUp(float pos, Transform character)
  {
    if (!_innerContainer)
    {
      _innerContainer = new GameObject("First Person View").transform;
      _innerContainer.SetPositionAndRotation(transform.position, transform.rotation);
      _innerContainer.parent = transform;
      _mainCam.transform.parent = _innerContainer;
      _fpvCam.transform.parent = _innerContainer;
    }

    _innerContainer.localPosition = new Vector3(0f, pos, 0f);
    character.parent = _innerContainer;

    _mainCam.transform.SetPositionAndRotation(_innerContainer.position, _innerContainer.rotation);
    _fpvCam.transform.SetPositionAndRotation(_innerContainer.position, _innerContainer.rotation);

    _core.Fpv.StartUp();
  }

  private void Update()
  {
    if (!_core.IsLocked)
    {
      MouseLook();
      PlayerMove();
    }
  }

  private void PlayerMove()
  {
    _velocity.x = _velocity.z = 0;

    if (_char.isGrounded)
    {
      _velocity.y = 0f;

      if (Input.GetButtonDown("Jump"))
        _velocity.y = _jumpSpeed;
    }

    _velocity += GetBaseInput() * _runSpeed;
    _velocity += Physics.gravity * 2f * Time.deltaTime;

    _char.Move(_velocity * Time.deltaTime);
    var state = GameState.GetInstance();
    state.localPlayer.posX = transform.position.x;
    state.localPlayer.posY = transform.position.y;
    state.localPlayer.posZ = transform.position.z;
  }

  private void MouseLook()
  {
    _lookRotation.y += Input.GetAxis("Mouse X") * _lookSpeed;
    _lookRotation.x += -Input.GetAxis("Mouse Y") * _lookSpeed;
    _lookRotation.x = Mathf.Clamp(_lookRotation.x, -_lookClamp * _lookSpeed, _lookClamp * _lookSpeed);
    transform.rotation = Quaternion.Euler(0, _lookRotation.y, 0);
    _innerContainer.localRotation = Quaternion.Euler(_lookRotation.x, 0, 0);
    var state = GameState.GetInstance();
    state.localPlayer.rotX = _lookRotation.x;
    state.localPlayer.rotY = _lookRotation.y;
  }

  private Vector3 GetBaseInput()
  {
    _moveDirection = Vector3.zero;
    _moveDirection += Input.GetAxis("Vertical") * transform.forward
    + Input.GetAxis("Horizontal") * transform.right;
    if (_moveDirection != Vector3.zero && _moveDirection.magnitude > 1)
      _moveDirection.Normalize();

    return _moveDirection;
  }

  private Camera CreateFpvCamera(Camera cam)
  {
    Camera fpvCam = Instantiate(_mainCam);
    Destroy(fpvCam.gameObject.GetComponent<AudioListener>());

    fpvCam.name = "Fpv Camera";
    fpvCam.gameObject.tag = "Untagged";
    fpvCam.depth = 1;
    fpvCam.clearFlags = CameraClearFlags.Depth;
    fpvCam.cullingMask = 1 << 9;

    return fpvCam;
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

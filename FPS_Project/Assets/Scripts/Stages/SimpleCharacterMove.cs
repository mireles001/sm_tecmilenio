using UnityEngine;

public class SimpleCharacterMove : MonoBehaviour
{
  public float _runSpeed = 0f;
  public float _jumpForce = 0f;
  public float _mouseSensitivity = 5f;
  private float _lookClamp;
  private Vector2 _lookRotation = Vector2.zero;
  private Vector3 _moveDirection;
  private Vector3 _velocity = new Vector3();
  public Transform _firstViewContainer;
  private CharacterController _char;

  private void Start()
  {
    _char = GetComponent<CharacterController>();
    _lookClamp = 80f / _mouseSensitivity;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update()
  {
    MouseLook();
    PlayerMove();
  }

  private void PlayerMove()
  {
    _velocity.x = _velocity.z = 0;

    if (_char.isGrounded)
    {
      _velocity.y = 0f;

      if (Input.GetButtonDown("Jump"))
        _velocity.y = _jumpForce;
    }

    _velocity += GetBaseInput() * _runSpeed;
    _velocity += Physics.gravity * 2f * Time.deltaTime;

    _char.Move(_velocity * Time.deltaTime);
  }

  private void MouseLook()
  {
    _lookRotation.y += Input.GetAxis("Mouse X");
    _lookRotation.x += -Input.GetAxis("Mouse Y");
    _lookRotation.x = Mathf.Clamp(_lookRotation.x, -_lookClamp, _lookClamp);
    transform.eulerAngles = new Vector2(0, _lookRotation.y) * _mouseSensitivity;
    _firstViewContainer.localRotation = Quaternion.Euler(_lookRotation.x * _mouseSensitivity, 0, 0);
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
}

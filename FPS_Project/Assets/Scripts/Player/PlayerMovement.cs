using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  // Editable stats
  public float jumpSpeed = 4f;
  public float runSpeed = 4f;
  public float playerHp = 100f;
  public float camSens = 0.25f;
  public Transform cameraPos;

  private Vector3 _velocity = new Vector3();
  private Vector3 _lastMouse = new Vector3(255, 255, 255);
  
  private bool _isLocked = false;
  private CharacterController _char;
  private FpvAnimation _fpv;

  private void Start()
  {
    Camera.main.transform.parent = cameraPos;
    Camera.main.transform.localPosition = Vector3.zero;
    Camera.main.transform.localRotation = Quaternion.identity;
    _char = GetComponent<CharacterController>();
  }

  private void LateUpdate()
  {
    if (!_isLocked)
    {
      _lastMouse = Input.mousePosition - _lastMouse;
      _lastMouse = new Vector3(-_lastMouse.y * camSens, _lastMouse.x * camSens, 0);

      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _lastMouse.y, 0);
      cameraPos.eulerAngles = new Vector3(cameraPos.eulerAngles.x + _lastMouse.x, cameraPos.eulerAngles.y, 0);
      _lastMouse = Input.mousePosition;

      if (_char.isGrounded && Input.GetButtonDown("Jump"))
      {
        _velocity.y = jumpSpeed;
      }

      _velocity.x = 0;
      _velocity.z = 0;
      _velocity += GetBaseInput() * runSpeed;

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

  public bool IsLocked
  {
    get
    {
      return _isLocked;
    }
  }
}

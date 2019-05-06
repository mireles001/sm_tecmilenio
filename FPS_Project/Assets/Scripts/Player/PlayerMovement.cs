using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  // editable stats
  public float jumpSpeed = 4;
  public float runSpeed = 4;
  public float playerHp = 100;
  public bool isMain = true;

  // values
  private float _camSens = 0.25f; //How sensitive it with mouse
  // float airAccRatio = 5;

  // state
  // [HideInInspector]
  private Vector3 _velocity = new Vector3();
  private Vector3 _inputVelocity = new Vector3();
  private Vector3 _lastMouse = new Vector3(255, 255, 255);
  private bool _jumpPressed = false;
  private bool _jumpHold = false;

  // unity reference
  public CharacterController charControl;
  public Transform cameraPos;

  // Start is called before the first frame update
  void Start()
  {


  }

  // Update is called once per frame
  void Update()
  {
    //Mouse  camera angle
    _lastMouse = Input.mousePosition - _lastMouse;
    _lastMouse = new Vector3(-_lastMouse.y * _camSens, _lastMouse.x * _camSens, 0);
    // _lastMouse = new Vector3(transform.eulerAngles.x + _lastMouse.x, transform.eulerAngles.y + _lastMouse.y, 0);
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _lastMouse.y, 0);
    cameraPos.eulerAngles = new Vector3(cameraPos.eulerAngles.x + _lastMouse.x, cameraPos.eulerAngles.y, 0);
    _lastMouse = Input.mousePosition;

    if (charControl.isGrounded)
    {
      _velocity.y = 0;
      if (_jumpPressed) {
        _velocity.y = jumpSpeed;
      }
    } else {

    }
      
    _velocity.x = 0;
    _velocity.z = 0;
    _velocity += GetBaseInput() * runSpeed;

    _velocity += Physics.gravity * Time.deltaTime;
    
    charControl.Move(_velocity * Time.deltaTime );

    Camera.main.transform.position = cameraPos.position;
    Camera.main.transform.rotation = cameraPos.rotation;
  }

  private Vector3 GetBaseInput()
  { 
    _jumpPressed = Input.GetButtonDown("Jump");
    _jumpHold = Input.GetButton("Jump");

    //returns the basic values, if it's 0 than it's not active.
    Vector3 moveDirection = new Vector3();
    moveDirection += Input.GetAxis("Vertical") * transform.forward
    + Input.GetAxis("Horizontal") * transform.right;
    moveDirection.y = 0;
    moveDirection.Normalize();
    return moveDirection;
  }

  void SetCursorState()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  public void SetFpv(FpvAnimation fpv)
  {
    fpv.StartUp(this);
  }
}

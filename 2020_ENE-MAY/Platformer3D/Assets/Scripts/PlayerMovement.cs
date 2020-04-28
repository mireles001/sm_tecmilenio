using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public bool IsGrounded { get; private set; } = true;
  public float Direction { get; protected set; } = 0f;
  public Rigidbody Rb { get; private set; }

  private bool _isLocked = false;
  [SerializeField]
  private float _moveSpeed = 2f;
  [SerializeField, Range(0f, 1f)]
  private float _midAirMoveDrag = 0.5f;
  [SerializeField]
  private float _jumpForce = 4f;

  private GameObject _gameManager;
  private PlayerAnimation _playerAnim;

  private void Start()
  {
    Rb = GetComponent<Rigidbody>();
    _playerAnim = GetComponent<PlayerAnimation>();
    _gameManager = GameObject.FindGameObjectWithTag("GameController");
  }

  private void Update()
  {
    if (_isLocked) return;

    PlayerInput();
  }

  private void FixedUpdate()
  {
    if (Direction != 0)
    {
      Move(Time.fixedDeltaTime);
    }
  }

  // Keyboard input manager
  protected virtual void PlayerInput()
  {
    // Move
    Direction = Input.GetAxis("Horizontal");

    // Jump
    if (Input.GetButtonDown("Jump"))
    {
      Jump();
    }
  }

  private void Move(float time)
  {
    Vector3 newVelocity = Rb.velocity;

    if (IsGrounded || (!IsGrounded && _midAirMoveDrag == 0))
    {
      newVelocity.x = Direction * _moveSpeed;
    }
    else
    {
      newVelocity.x += (Direction * _moveSpeed * time) / _midAirMoveDrag;
      newVelocity.x = Mathf.Clamp(newVelocity.x, -_moveSpeed, _moveSpeed);
    }

    Rb.velocity = newVelocity;
  }

  private void Jump()
  {
    if (IsGrounded)
    {
      IsGrounded = false;
      _playerAnim.Jump();
      Rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
  }

  private void OnTriggerStay(Collider collision)
  {
    IsGrounded = true;
  }

  private void OnTriggerExit(Collider collision)
  {
    IsGrounded = false;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.tag == "Death" && !_isLocked)
    {
      Death();
    }
    else if (collision.gameObject.tag == "Respawn" && !_isLocked)
    {
      collision.gameObject.SendMessage("Use", _gameManager, SendMessageOptions.DontRequireReceiver);
    }
    else if (collision.gameObject.tag == "Finish" && !_isLocked)
    {
      LockPlayer();
      _gameManager.SendMessage("Finish", SendMessageOptions.DontRequireReceiver);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Death" && !_isLocked)
    {
      Death();
    }
  }

  private void Death()
  {
    LockPlayer();
    _playerAnim.Death();
    _gameManager.SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
  }

  private void LockPlayer()
  {
    _isLocked = true;
    Direction = 0;
  }
}

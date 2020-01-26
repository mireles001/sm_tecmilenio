using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public bool IsGrounded { get; private set; } = false;
  public float Direction { get; protected set; } = 0f;
  public Rigidbody2D Rb { get; private set; }

  private bool _isLocked = false;
  [SerializeField]
  private float _moveSpeed = 2f;
  [SerializeField, Range(0f, 1f)]
  private float _midAirMoveDrag = 0.5f;
  [SerializeField]
  private float _jumpForce = 4f;

  private void Start()
  {
    Rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (_isLocked)
    {
      return;
    }

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
    Vector2 newVelocity = Rb.velocity;

    if (IsGrounded || (!IsGrounded && _midAirMoveDrag == 0f))
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

  protected void JumpHandler()
  {
    if (IsGrounded)
    {
      SendMessage("Jump");
    }
  }

  public void Jump()
  {
    Rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    IsGrounded = true;
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    IsGrounded = false;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Respawn" && !_isLocked)
    {
      _isLocked = true;
      GameObject.FindGameObjectWithTag("GameController").SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
    }
  }
}

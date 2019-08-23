using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
  [SerializeField]
  private float _moveSpeed = 5f;
  [SerializeField]
  private float _jumpForce = 10f;
  public bool IsGrounded { get; private set; } = false;
  public Vector3 Velocity { get; private set; } = Vector3.zero;
  private Vector3 _direction;
  private Vector3 _lastPosition;
  [SerializeField]
  private LayerMask _scenarioLayerMask = ~0;
  private Rigidbody _rb;
  private Transform _cam;
  private PlayerInput _input;
  private CharacterAnimation _charAnimations;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody>();
    _input = GetComponent<PlayerInput>();
  }

  private void Start()
  {
    _cam = Camera.main.transform;
  }

  private void Update()
  {
    GetMovementDirection();
    Rotate();
  }

  private void FixedUpdate()
  {
    Move();
    GetVelocity(Time.fixedDeltaTime);
  }

  private void Move()
  {
    if (_direction == Vector3.zero)
    {
      return;
    }

    _rb.MovePosition(transform.position + _direction * Time.deltaTime * _moveSpeed);
  }

  private void Rotate()
  {
    if (_direction == Vector3.zero)
    {
      return;
    }

    transform.forward = _direction;
  }

  public void Jump()
  {
    if (!IsGrounded)
    {
      return;
    }

    if (_charAnimations)
    {
      _charAnimations.PlayJump();
    }

    if (Mathf.Abs(_rb.velocity.y) > 0.01f)
    {
      Vector3 velocity = _rb.velocity;
      velocity.y = 0f;
      _rb.velocity = velocity;
    }
    _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
  }

  private void GetMovementDirection()
  {
    if (_input.MoveDirection == Vector3.zero)
    {
      _direction = Vector3.zero;
      return;
    }

    Vector3 camForward = _cam.forward;
    Vector3 camRight = _cam.right;

    camForward.y = 0;
    camRight.y = 0;
    camForward = camForward.normalized;
    camRight = camRight.normalized;

    _direction = (camForward * _input.MoveDirection.z + camRight * _input.MoveDirection.x);
  }

  private Vector3 GetVelocity(float timeScale)
  {
    Vector3 velocity = (_rb.position - _lastPosition) / timeScale;
    _lastPosition = _rb.position;

    return Velocity = velocity;
  }

  private void OnTriggerStay(Collider other)
  {
    if (!IsGrounded && ((1 << other.gameObject.layer) & _scenarioLayerMask) != 0)
    {
      IsGrounded = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (IsGrounded && ((1 << other.gameObject.layer) & _scenarioLayerMask) != 0)
    {
      IsGrounded = false;
    }
  }

  public void SetCharacterAnimations(CharacterAnimation target)
  {
    _charAnimations = target;
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, Screen.height - 30, 300, 20), "Velocity: " + Velocity);
  }
}

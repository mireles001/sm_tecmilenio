using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
  public float speed = 5f;
  public float jumpForce = 10f;
  public CharacterAnimation characterAnimation;

  private bool _isGrounded = false;
  private Rigidbody _rb;
  private Transform _cam;

  private Vector3 _direction;

  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _cam = Camera.main.transform;
  }

  private void Update()
  {
    if (Input.GetButtonDown("Jump") && _isGrounded)
    {
      _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

      if (characterAnimation)
      {
        characterAnimation.Jump();
      }
    }

    float xAxis = Input.GetAxis("Horizontal");
    float yAxis = Input.GetAxis("Vertical");

    if (xAxis != 0 || yAxis != 0)
    {
      Vector3 camForward = _cam.forward;
      Vector3 camRight = _cam.right;

      camForward.y = 0;
      camRight.y = 0;
      camForward = camForward.normalized;
      camRight = camRight.normalized;

      _direction = (camForward * yAxis + camRight * xAxis);

      transform.forward = _direction;

      if (_direction.magnitude > 1f)
      {
        _direction = _direction.normalized;
      }
    }
    else
    {
      _direction = Vector3.zero;
    }

    if (characterAnimation)
    {
      characterAnimation.MoveSpeed = Mathf.Max(Mathf.Abs(xAxis), Mathf.Abs(yAxis));
      characterAnimation.VerticalSpeed = _rb.velocity.y;
      characterAnimation.IsGrounded = _isGrounded;
    }
  }

  private void FixedUpdate()
  {
    _rb.MovePosition(transform.position + _direction * Time.fixedDeltaTime * speed);
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.tag != "Player")
    {
      _isGrounded = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag != "Player")
    {
      _isGrounded = false;
    }
  }
}

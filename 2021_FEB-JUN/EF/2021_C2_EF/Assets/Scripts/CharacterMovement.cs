using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 7;


    private bool _isGrounded = false;
    private Vector3 _direction;
    private Rigidbody _rb;
    private CharacterAnimation _characterAnimation;

    [SerializeField]
    private CameraMovement _cameraWrapper = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _characterAnimation = GetComponent<CharacterAnimation>();

        if (!_cameraWrapper) return;

        _cameraWrapper.Target = transform;
        Vector3 lootAt = transform.position;
        lootAt += Vector3.up * GetComponent<CapsuleCollider>().height * 0.75f;
        _cameraWrapper.SetLookTarget(lootAt);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            if (_characterAnimation) _characterAnimation.Jump();
        }

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        if (xAxis != 0 || yAxis != 0)
        {
            Vector3 camForward = _cameraWrapper.CameraTransform.forward;
            Vector3 camRight = _cameraWrapper.CameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            _direction = (camForward * yAxis + camRight * xAxis);

            transform.forward = _direction;

            if (_direction.magnitude > 1) _direction = _direction.normalized;
        }
        else _direction = Vector3.zero;

        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

        if (_characterAnimation)
        {
            _characterAnimation.MoveSpeed = Mathf.Max(Mathf.Abs(xAxis), Mathf.Abs(yAxis));
            _characterAnimation.VerticalSpeed = _rb.velocity.y;
            _characterAnimation.IsGrounded = _isGrounded;
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

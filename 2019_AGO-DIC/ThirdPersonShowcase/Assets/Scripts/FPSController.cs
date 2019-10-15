using UnityEngine;

public class FPSController : MonoBehaviour
{
  [SerializeField]
  private float _look = 100f;
  [SerializeField]
  private float _speed = 10f;
  [SerializeField]
  private float _jump = 20f;
  [SerializeField]
  private LayerMask _scenarioLayerMask = ~0;

  private bool _isGrounded = false;
  private float rotX;
  private float rotY;
  private Rigidbody _rb;
  private Camera _cam;

  private void Awake()
  {
    Physics.gravity = new Vector3(0, -16f, 0);
  }

  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _cam = Camera.main;

    Vector3 rotPlayer = transform.localRotation.eulerAngles;
    rotY = rotPlayer.y;
    Vector3 rotCamera = _cam.transform.localRotation.eulerAngles;
    rotX = rotCamera.x;
  }

  private void Update()
  {
    Move();
    Look();
    Jump();
  }

  private void Move()
  {
    float xAxis = Input.GetAxis("Horizontal");
    float yAxis = Input.GetAxis("Vertical");

    if (xAxis != 0 || yAxis != 0)
    {
      Vector3 direction = transform.forward * yAxis + transform.right * xAxis;

      if (direction.magnitude > 1)
      {
        direction.Normalize();
      }

      transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }
  }

  private void Look()
  {
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = -Input.GetAxis("Mouse Y");

    rotY += mouseX * _look * Time.deltaTime;
    rotX += mouseY * _look * Time.deltaTime;

    transform.rotation = Quaternion.Euler(0f, rotY, 0f);
    _cam.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
  }

  private void Jump()
  {
    if (_isGrounded && Input.GetButtonDown("Jump"))
    {
      if (_rb.velocity.y != 0f)
      {
        Vector3 velocity = _rb.velocity;
        velocity.y = 0f;
        _rb.velocity = velocity;
      }
      _rb.AddForce(Vector3.up * _jump, ForceMode.Impulse);
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (!_isGrounded && ((1 << other.gameObject.layer) & _scenarioLayerMask) != 0)
    {
      _isGrounded = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (_isGrounded && ((1 << other.gameObject.layer) & _scenarioLayerMask) != 0)
    {
      _isGrounded = false;
    }
  }
}

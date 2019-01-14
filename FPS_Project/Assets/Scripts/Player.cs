using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
  private static float globalGravity = -9.81f;
  private float gravityScale = 4.0f;

  private float _speed = 10f;
  private float _jump = 10f;
  private Rigidbody _rb;

  private NetworkCore _network;

  public Text _username;

  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _rb.useGravity = false;
    SetUsername("unnamed");
  }

  public void StartUp(GameObject networkGo)
  {
    _network = networkGo.GetComponent<NetworkCore>();
  }

  private void Update()
  {
    Vector3 rot = transform.eulerAngles;
    rot.y += Input.GetAxis("Horizontal") * (_speed / 2f);
    transform.eulerAngles = rot;

    if (Input.GetButtonDown("Jump"))
      _rb.AddForce(transform.up * _jump, ForceMode.Impulse);
  }

  private void FixedUpdate()
  {
    Vector3 gravity = globalGravity * gravityScale * Vector3.up;
    _rb.AddForce(gravity, ForceMode.Acceleration);

    Vector3 newPosition = transform.position + (transform.forward * Input.GetAxis("Vertical")) * _speed * Time.fixedDeltaTime;

    _rb.MovePosition(newPosition);
  }

  public void SetUsername(string username)
  {
    _username.text = username;
  }
}

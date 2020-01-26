using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public Transform target;

  [SerializeField]
  private float _damp = 1f;
  [SerializeField, Range(0, 1)]
  private float _lookAt = 1f;
  private Vector3 _targetHeight;
  private Vector3 _velocity;

  private void Start()
  {
    _targetHeight = Vector3.up * target.GetComponent<CapsuleCollider2D>().bounds.size.y * _lookAt;
  }

  private void Update()
  {
    Vector3 newPosition = target.position + _targetHeight;
    newPosition.z = transform.position.z;

    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _damp);
  }
}

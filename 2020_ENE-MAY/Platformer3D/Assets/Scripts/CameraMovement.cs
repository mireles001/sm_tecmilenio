using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public Transform target;
  [SerializeField]
  private bool _changeHeight = true;
  [SerializeField]
  private float _damp = 1f;
  private float _heightOffset;
  private Vector3 _velocity;

  private void Start()
  {
    _heightOffset = Mathf.Abs(transform.position.y - target.position.y);
    transform.position = GetNewPosition();
  }

  private void Update()
  {
    transform.position = Vector3.SmoothDamp(transform.position, GetNewPosition(), ref _velocity, _damp);
  }

  private Vector3 GetNewPosition()
  {
    Vector3 newPosition = target.position;
    newPosition.z = transform.position.z;

    if (_changeHeight)
    {
      newPosition.y = target.position.y + _heightOffset;
    }
    else
    {
      newPosition.y = transform.position.y;
    }

    return newPosition;
  }
}

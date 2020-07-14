using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  [SerializeField]
  private Transform _target = null;
  [SerializeField]
  private float _damp = 1;
  private Vector3 _velocity;

  private void Update()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = _target.position.x;

    if (_damp <= 0)
    {
      transform.position = newPosition;
    }
    else
    {
      transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _damp);
    }
  }
}

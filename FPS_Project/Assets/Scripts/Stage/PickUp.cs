using UnityEngine;

public class PickUp : MonoBehaviour
{
  private enum PickUpType
  {
    health,
    grenades,
    rockets
  }
  [SerializeField]
  private PickUpType _characterType = PickUpType.health;
  [SerializeField]
  private int _amount = 1;
  [SerializeField]
  private Transform _mesh;

  [SerializeField]
  private float _spinSpeed = 10f;
  private Vector3 _rotation;

  [SerializeField]
  private float _oscillation = 0.25f;
  [SerializeField]
  private float _oscillationSpeed = 1f;
  private PickUpSpawner _spawner;
  private float _oscillationFraction = 0f;
  private Vector3 _startPos;
  private Vector3 _endPos;
  private Vector3 _currentPos;

  private void Start()
  {
    if (!_mesh)
    {
      _mesh = transform;
    }

    _startPos = _mesh.position;
    _startPos.y -= _oscillation;
    _endPos = _mesh.position;
    _endPos.y += _oscillation;

    _currentPos = _mesh.position;
    _currentPos.y = Random.Range(_startPos.y, _endPos.y);
    _mesh.position = _currentPos;

    float currDistance = Vector3.Distance(_currentPos, _endPos);
    float totalDistance = Vector3.Distance(_startPos, _endPos);
    _oscillationFraction = currDistance / totalDistance;

    _rotation = _mesh.eulerAngles;
    _rotation.y = Random.Range(0, 359);
    _mesh.eulerAngles = _rotation;

  }

  public void StartUp(PickUpSpawner spawner)
  {
    _spawner = spawner;
  }
  private void Update()
  {
    _rotation.y += Time.deltaTime * _spinSpeed;

    if (_oscillationFraction < 1)
    {
      _oscillationFraction += Time.deltaTime * _oscillationSpeed;
      _currentPos = Vector3.Lerp(_startPos, _endPos, _oscillationFraction);
    }
    else
    {
      Vector3 tempVector = _startPos;
      _startPos = _endPos;
      _endPos = tempVector;

      _oscillationFraction = 0f;
    }

    _mesh.SetPositionAndRotation(_currentPos, Quaternion.Euler(_rotation));
  }

  private void PickedUp(GameObject player)
  {
    _spawner.PickedUp();
    player.GetComponent<PlayerCore>().GotPickUp(_characterType.GetHashCode(), _amount);

    Destroy(gameObject);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player")
    {
      PickedUp(other.transform.gameObject);
    }
  }
}

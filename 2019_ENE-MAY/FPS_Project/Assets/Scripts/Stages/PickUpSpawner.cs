using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
  [SerializeField]
  private GameObject _respawnPrefab;
  [SerializeField]
  private float _respawnCooldown = 30f;
  private bool _itemExists = false;
  private float _currentTimer;

  private void Awake()
  {
    if (!_respawnPrefab)
    {
      _respawnPrefab = new GameObject("empty");
      _itemExists = true;
    }

    _currentTimer = _respawnCooldown;
  }

  private void Update()
  {
    if (!_itemExists)
    {
      _currentTimer += Time.deltaTime;

      if (_respawnCooldown < _currentTimer)
      {
        Respawn();
      }
    }
  }

  private void Respawn()
  {
    _itemExists = true;

    GameObject respawn = Instantiate(_respawnPrefab, transform);
    respawn.transform.SetPositionAndRotation(transform.position, transform.rotation);
    respawn.GetComponent<PickUp>().StartUp(this);
  }

  public void PickedUp()
  {
    _itemExists = false;
    _currentTimer = 0f;
  }
}

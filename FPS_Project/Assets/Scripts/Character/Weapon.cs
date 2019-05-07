using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField]
  private Transform _projectileSpawner;

  public Transform ProjectileSpawner
  {
    get
    {
      return _projectileSpawner;
    }
    set
    {
      _projectileSpawner = value;
    }
  }
}

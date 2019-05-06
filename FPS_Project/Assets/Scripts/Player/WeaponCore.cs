using UnityEngine;

public class WeaponCore : MonoBehaviour
{
  [SerializeField]
  private Transform _weaponFireSpawn;

  public Transform WeaponFireSpawn
  {
    get
    {
      return _weaponFireSpawn;
    }
  }
}

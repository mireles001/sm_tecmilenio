using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
  public GameObject[] weapons;
  [SerializeField]
  private Transform _weaponHolder;
  private GameObject _currentWeapon;
  private PlayerWeapons _player;
  private int _weaponIndex;

  public WeaponsManager StartUp(PlayerWeapons player)
  {
    _player = player;
    return this;
  }

  public void WeaponChange(int index)
  {
    _weaponIndex = index;
    Invoke("WeaponChangeInvoke", 0.33f);
  }

  private void WeaponChangeInvoke()
  {
    if (_currentWeapon)
      Destroy(_currentWeapon);

    GameObject weapon = Instantiate(weapons[_weaponIndex - 1]);
    weapon.name = "weapon_" + _weaponIndex;
    weapon.transform.parent = _weaponHolder;
    weapon.transform.localPosition = Vector3.zero;
    weapon.transform.localRotation = Quaternion.identity;

    _currentWeapon = weapon;

    // This only runs when its FirstPersonView
    if (_player)
    {
      _player.SetWeaponFireSpawn(weapon.GetComponent<WeaponCore>().WeaponFireSpawn);

      DisableCastShadow(_currentWeapon.transform);
    }
  }

  private void DisableCastShadow(Transform target)
  {
    int children = target.childCount;
    for (int i = 0; i < children; ++i)
    {
      MeshRenderer rend = target.GetChild(i).GetComponent<MeshRenderer>();

      if (rend)
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

      DisableCastShadow(target.GetChild(i));
    }
  }
}

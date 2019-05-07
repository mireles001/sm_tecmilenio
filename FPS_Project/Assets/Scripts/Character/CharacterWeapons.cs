using UnityEngine;

public class CharacterWeapons : MonoBehaviour
{
  [SerializeField]
  private GameObject[] _weapons;
  [SerializeField]
  private Transform _weaponHolder;
  [SerializeField]
  private float _weaponSwapTimer = 0.33f;
  private bool _isPlayer = false;
  private int _weaponIndex;
  private GameObject _currentWeapon;
  private CharacterCore _core;

  private void Start()
  {
    // TODO: Make this selectable
    //_isPlayer = true;

    if (_isPlayer)
    {
      _core = GetComponent<CharacterCore>();
    }
  }

  public void WeaponChange(int index)
  {
    if (_weapons.Length > 0)
    {
      _weaponIndex = index;
      Invoke("WeaponChangeInvoke", _weaponSwapTimer);
    }
  }

  private void WeaponChangeInvoke()
  {
    if (_currentWeapon)
      Destroy(_currentWeapon);

    _currentWeapon = Instantiate(_weapons[_weaponIndex - 1]);
    _currentWeapon.transform.parent = _weaponHolder;
    _currentWeapon.transform.localPosition = Vector3.zero;
    _currentWeapon.transform.localRotation = Quaternion.identity;

    // This only runs when its FirstPersonView
    if (_isPlayer)
    {
      _core.PlayerCore.PlayerWeapons.ProjectileSpawner = _currentWeapon.GetComponent<Weapon>().ProjectileSpawner;

      DisableCastShadow(_currentWeapon.transform);
    }
  }

  // Disable Cast Shadow for GameObject and children (recursive)
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

using UnityEngine;

public class CharacterWeapons : MonoBehaviour
{
  private enum CharacterType
  {
    thirdPerson,
    firstPerson
  }
  [SerializeField]
  private GameObject[] _weapons;
  [SerializeField]
  private Transform _weaponHolder;
  [SerializeField]
  private float _weaponSwapTimer = 0.33f;
  [SerializeField]
  private CharacterType _characterType;
  private bool _isPlayer;
  private int _weaponIndex;
  private GameObject _currentWeapon;
  private CharacterCore _core;

  private void Start()
  {
    switch (_characterType.GetHashCode())
    {
      case 0:
        _isPlayer = false;
        break;
      case 1:
        _isPlayer = true;
        _core = GetComponent<CharacterCore>();
        break;
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
      {
        rend.gameObject.layer = 9;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      }

      DisableCastShadow(target.GetChild(i));
    }
  }

  public float WeaponSwapTimer
  {
    get
    {
      return _weaponSwapTimer;
    }
    set
    {
      _weaponSwapTimer = value;
    }
  }
}

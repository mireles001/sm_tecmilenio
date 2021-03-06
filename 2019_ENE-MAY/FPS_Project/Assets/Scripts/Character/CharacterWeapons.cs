﻿using UnityEngine;

public class CharacterWeapons : MonoBehaviour
{
  private enum CharacterType
  {
    thirdPerson,
    firstPerson
  }
  [SerializeField]
  private CharacterType _characterType = CharacterType.firstPerson;
  [SerializeField]
  private GameObject[] _weapons = new GameObject[3];
  [SerializeField]
  private Transform _weaponHolder;

  [SerializeField]
  private float _weaponSwapTimer = 0.33f;
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

    if (!_weaponHolder)
    {
      _weaponHolder = transform;
    }

    if (_isPlayer)
    {
      OffscreenUpdater(transform);
    }
  }

  private void OffscreenUpdater(Transform target)
  {
    int children = target.childCount;
    for (int i = 0; i < children; ++i)
    {
      SkinnedMeshRenderer rend = target.GetChild(i).GetComponent<SkinnedMeshRenderer>();

      if (rend)
      {
        rend.gameObject.layer = 9;
        rend.updateWhenOffscreen = true;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
      }

      OffscreenUpdater(target.GetChild(i));
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
    {
      Destroy(_currentWeapon);
    }

    _currentWeapon = Instantiate(_weapons[_weaponIndex - 1]);
    _currentWeapon.transform.parent = _weaponHolder;
    _currentWeapon.transform.localPosition = Vector3.zero;
    _currentWeapon.transform.localRotation = Quaternion.identity;

    if (_isPlayer)
    {
      _core.PlayerCore.PlayerWeapons.ProjectileSpawner = _currentWeapon.GetComponent<Weapon>().ProjectileSpawner;
      ChildrenRendererMod(_currentWeapon.transform);
    }
  }

  // Disable Cast Shadow for GameObject and children (recursive)
  private void ChildrenRendererMod(Transform target)
  {
    int children = target.childCount;
    for (int i = 0; i < children; ++i)
    {
      MeshRenderer rend = target.GetChild(i).GetComponent<MeshRenderer>();

      if (rend)
      {
        rend.gameObject.layer = 9;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
      }

      ChildrenRendererMod(target.GetChild(i));
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

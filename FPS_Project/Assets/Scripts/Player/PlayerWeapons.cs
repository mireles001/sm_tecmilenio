using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
  [SerializeField]
  private int _grenades = 0;
  [SerializeField]
  private int _rockets = 0;
  private int _weaponIndex = 0;
  private Transform _projectileSpawner;
  private PlayerCore _core;

  private void Awake()
  {
    _core = GetComponent<PlayerCore>();
  }

  public void StartUp()
  {
    ChangeWeapon(1);
  }

  private void LateUpdate()
  {
    if (!_core.IsLocked)
    {
      int changeWeapon = 0;
      if (Input.GetKeyDown("1"))
        changeWeapon = 1;
      else if (Input.GetKeyDown("2"))
        changeWeapon = 2;
      else if (Input.GetKeyDown("3"))
        changeWeapon = 3;
      else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
      {
        changeWeapon = _weaponIndex;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
          changeWeapon++;
          if (changeWeapon > 3)
            changeWeapon = 1;
        }
        else
        {
          changeWeapon--;
          if (changeWeapon < 1)
            changeWeapon = 3;
        }
      }

      if (changeWeapon > 0 && changeWeapon != _weaponIndex)
        ChangeWeapon(changeWeapon);

      if (Input.GetButtonDown("Fire1"))
      {
        if (HaveAmmo())
          WeaponUse(_weaponIndex);
        else
          ChangeWeapon(1);
      }
    }
  }

  private void ChangeWeapon(int index)
  {
    bool validWeapon = false;
    if (index == 1)
      validWeapon = true;
    else if ((index == 2 && _grenades > 0) || (index == 3 && _rockets > 0))
      validWeapon = true;

    if (validWeapon)
    {
      Debug.Log("Change weapon: " + index);
      _weaponIndex = index;
      _core.Fpv.WeaponChange(index);
    }
  }

  private bool HaveAmmo()
  {
    bool shoot = false;

    if (_weaponIndex == 1 || (_weaponIndex == 2 && _grenades > 0) || (_weaponIndex == 3 && _rockets > 0))
      shoot = true;

    return shoot;
  }

  private void WeaponUse(int index)
  {
    switch (index)
    {
      case 2:
        _grenades--;
        break;
      case 3:
        _rockets--;
        break;
      default:
        break;
    }

    Debug.Log("Shoot weapon: " + index + " at " + _projectileSpawner.parent.gameObject.name);
    _core.Fpv.WeaponUse();
  }

  public int Grenades
  {
    get
    {
      return _grenades;
    }
    set
    {
      _grenades = value;
    }
  }

  public int Rockets
  {
    get
    {
      return _rockets;
    }
    set
    {
      _rockets = value;
    }
  }

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

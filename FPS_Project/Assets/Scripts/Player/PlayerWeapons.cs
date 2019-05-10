using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
  [SerializeField]
  private int _grenades = 0;
  [SerializeField]
  private int _rockets = 0;
  [SerializeField]
  private float _machineGunFireRate = 0.25f;
  [SerializeField]
  private float _grenadeFireRate = 0.7f;
  [SerializeField]
  private float _rocketFireRate = 0.6f;
  private bool _weaponLocked = false;
  private float _totalWait;
  private float _currentWait;
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

  private void Update()
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

      if (!_weaponLocked)
      {
        if (changeWeapon > 0 && changeWeapon != _weaponIndex)
          ChangeWeapon(changeWeapon);
        else if (Input.GetButton("Fire1"))
        {
          if (HaveAmmo())
            WeaponUse(_weaponIndex);
          else
          {
            _weaponLocked = true;
            _currentWait = 0f;
            _totalWait = _grenadeFireRate;
            ChangeWeapon(1);
          }
        }
      }
      else
      {
        _currentWait += Time.deltaTime;
        if (_totalWait < _currentWait)
          _weaponLocked = false;
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
    _weaponLocked = true;
    _currentWait = 0f;

    switch (index)
    {
      case 2:
        _grenades--;
        _totalWait = _grenadeFireRate;
        break;
      case 3:
        _rockets--;
        _totalWait = _rocketFireRate;
        break;
      default:
        _totalWait = _machineGunFireRate;
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

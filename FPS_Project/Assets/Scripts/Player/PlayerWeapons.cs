using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
  public int glAmmo = 5;
  public int rlAmmo = 5;
  private Transform _weaponFireSpawn;
  private int _weaponIndex = 0;
  private PlayerMovement _player;
  private PlayerCore _core;

  public void StartUp()
  {
    _core = GetComponent<PlayerCore>();
    _player = GetComponent<PlayerMovement>();

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
    else if ((index == 2 && glAmmo > 0) || (index == 3 && rlAmmo > 0))
      validWeapon = true;

    if (validWeapon)
    {
      Debug.Log("Change weapon: " + index);
      _weaponIndex = index;
      _player.Fpv.WeaponChange(index);
    }
  }

  private bool HaveAmmo()
  {
    bool shoot = false;

    if (_weaponIndex == 1 || (_weaponIndex == 2 && glAmmo > 0) || (_weaponIndex == 3 && rlAmmo > 0))
      shoot = true;

    return shoot;
  }

  private void WeaponUse(int index)
  {
    switch (index)
    {
      case 2:
        glAmmo--;
        break;
      case 3:
        rlAmmo--;
        break;
      default:
        break;
    }

    Debug.Log("Shoot weapon: " + index + " at " + _weaponFireSpawn.parent.gameObject.name);
    _player.Fpv.WeaponUse();
  }

  public void SetWeaponFireSpawn(Transform fireSpawn)
  {
    _weaponFireSpawn = fireSpawn;
  }
}

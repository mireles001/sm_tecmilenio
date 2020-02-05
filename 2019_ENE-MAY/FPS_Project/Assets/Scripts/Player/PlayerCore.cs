using UnityEngine;
using System.Collections.Generic;

public class PlayerCore : MonoBehaviour
{
  public GameObject _characterPrefab;
  public int _hp = 0;
  public int _maxHp = 0;

  private bool _isLocked = true;
  private string _characterName = "";
  private GameObject _character;
  private PlayerMovement _playerMove;
  private PlayerWeapons _playerWeapons;
  private FpvAnimation _fpv;
  private CharacterSFX _sfx;
  private GameMaster _master;

  private void Awake()
  {
    _playerMove = GetComponent<PlayerMovement>();
    _playerWeapons = GetComponent<PlayerWeapons>();
  }

  private void Start()
  {
    GameObject masterGo = GameObject.FindGameObjectWithTag("GameMaster");

    if (masterGo)
    {
      _master = masterGo.GetComponent<GameMaster>();
    }
    else
    {
      LoadCharacter(_characterPrefab);
    }
  }

  public void LoadCharacter(GameObject prefab)
  {
    _isLocked = true;

    if (_character)
    {
      Destroy(_character);
    }

    _character = Instantiate(prefab, transform, false);
    _character.GetComponent<CharacterCore>().StartUp(this);
  }

  public void SetValues(Dictionary<string, object> characterParams)
  {
    _characterName = (string)characterParams["name"];
    _character.name = "fpv_" + _characterName;
    _maxHp = _hp = (int)characterParams["health"];
    _playerMove.RunSpeed = (float)characterParams["speed"];
    _playerMove.JumpSpeed = (float)characterParams["jump"];
    _playerMove.CharRb.radius = (float)characterParams["width"];
    _playerMove.CharRb.height = (float)characterParams["height"];
    _playerMove.CharRb.center = new Vector3(0f, _playerMove.CharRb.height / 2f, 0f);
    _fpv = (FpvAnimation)characterParams["fpv"];
    _playerMove.StartUp((float)characterParams["camera"], _character.transform);
    _playerWeapons.StartUp();

    ToggleControls(false);
  }

  public void RegisterDeath()
  {
    Debug.Log("Register Death by player");
    _sfx.Death();
  }

  public void ModifyHealth(int val)
  {
    _hp += val;

    if (_hp > _maxHp)
    {
      _hp = _maxHp;
    }
    else if (val < 0)
    {
      if (_hp <= 0)
      {
        RegisterDeath();
      }
      else
      {
        _sfx.Damage();
      }
    }
  }

  public bool GotPickUp(int pickUpType, int amount)
  {
    bool valid = false;

    switch (pickUpType)
    {
      case 1:
        if (_playerWeapons.Grenades < _playerWeapons.MaxGrenades)
        {
          _playerWeapons.Grenades += amount;
          if (_playerWeapons.Grenades > _playerWeapons.MaxGrenades)
          {
            _playerWeapons.Grenades = _playerWeapons.MaxGrenades;
          }
          _sfx.PickAmmo();
          valid = true;
        }

        break;
      case 2:
        if (_playerWeapons.Rockets < _playerWeapons.MaxRockets)
        {
          _playerWeapons.Rockets += amount;
          if (_playerWeapons.Rockets > _playerWeapons.MaxRockets)
          {
            _playerWeapons.Rockets = _playerWeapons.MaxRockets;
          }
          _sfx.PickAmmo();
          valid = true;
        }
        break;
      default:
        if (_hp < _maxHp)
        {
          ModifyHealth(amount);
          _sfx.PickHealth();
          valid = true;
        }
        break;
    }

    return valid;
  }

  public void ToggleControls(bool isLocked = false)
  {
    _isLocked = isLocked;

    if (_isLocked)
    {
      Cursor.lockState = CursorLockMode.None;
      Debug.Log("Character Locked");
    }
    else
    {
      Cursor.lockState = CursorLockMode.Locked;
      Debug.Log("Character Unlocked");
    }
  }

  #region GetterSetters
  public bool IsLocked
  {
    get
    {
      return _isLocked;
    }
  }

  public PlayerMovement PlayerMovement
  {
    get
    {
      return _playerMove;
    }
  }

  public PlayerWeapons PlayerWeapons
  {
    get
    {
      return _playerWeapons;
    }
  }

  public FpvAnimation Fpv
  {
    get
    {
      return _fpv;
    }
  }

  public CharacterSFX Sfx
  {
    get
    {
      return _sfx;
    }
    set
    {
      _sfx = value;
    }
  }
  #endregion
}

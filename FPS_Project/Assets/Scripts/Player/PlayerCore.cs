using UnityEngine;
using System.Collections.Generic;

public class PlayerCore : MonoBehaviour
{
  // TODO: This will be defined by a char selector
  public string characterPath;

  private bool _isLocked = true;
  [SerializeField]
  private string _characterName = "unnamed";
  [SerializeField]
  private int _hp = 0;
  [SerializeField]
  private int _maxHp = 0;
  private GameObject _character;
  private PlayerMovement _playerMove;
  private PlayerWeapons _playerWeapons;
  private FpvAnimation _fpv;

  private void Awake()
  {
    _playerMove = GetComponent<PlayerMovement>();
    _playerWeapons = GetComponent<PlayerWeapons>();
  }

  private void Start()
  {
    // TODO: Temporal internal call to character load
    LoadCharacter(characterPath);
  }

  // Called by GameMaster Object to load character content
  public void LoadCharacter(string path)
  {
    _isLocked = true;

    if (_character)
      Destroy(_character);

    _character = Instantiate(Resources.Load(characterPath + "/fpv") as GameObject, transform, false);
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
    _fpv = (FpvAnimation)characterParams["fpv"];
    _playerMove.StartUp((float)characterParams["camera"], _character.transform);
    _playerWeapons.StartUp();

    _isLocked = false;
  }

  public int Hp
  {
    get
    {
      return _hp;
    }
    set
    {
      _hp = value;
    }
  }

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
}

using UnityEngine;
using System.Collections.Generic;

public class CharacterCore : MonoBehaviour
{
  [SerializeField]
  private string _characterName = "unnamed";
  [SerializeField]
  private float _movementSpeed = 4f;
  [SerializeField]
  private float _jumpForce = 10f;
  [SerializeField]
  private int _maxHealth = 100;
  [SerializeField]
  private float _cameraPosition = 1.2f;
  [SerializeField]
  private float _colliderWidth = 0.3f;
  [SerializeField]
  private float _colliderHeight = 2f;
  private PlayerCore _core;
  private CharacterWeapons _weapons;
  private FpvAnimation _fpv;

  private void Awake()
  {
    _weapons = GetComponent<CharacterWeapons>();
    _fpv = GetComponent<FpvAnimation>();
  }

  public void StartUp(PlayerCore core)
  {
    _core = core;

    Dictionary<string, object> characterParams = new Dictionary<string, object>();

    characterParams["name"] = _characterName;
    characterParams["speed"] = _movementSpeed;
    characterParams["jump"] = _jumpForce;
    characterParams["health"] = _maxHealth;
    characterParams["camera"] = _cameraPosition;
    characterParams["width"] = _colliderWidth;
    characterParams["height"] = _colliderHeight;
    characterParams["fpv"] = _fpv;

    _core.SetValues(characterParams);
  }

  public PlayerCore PlayerCore
  {
    get
    {
      return _core;
    }
  }

  public string Name
  {
    get
    {
      return _characterName;
    }
  }

  public CharacterWeapons CharacterWeapons
  {
    get
    {
      return _weapons;
    }
  }
}

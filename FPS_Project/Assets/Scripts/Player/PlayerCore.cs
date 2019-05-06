using UnityEngine;
using System.Collections.Generic;

public class PlayerCore : MonoBehaviour
{
  public float hp;
  public float maxHp;

  // TODO: This will be defined by a char selector
  public string characterPath;
  private GameObject _character;
  private bool _characterLoaded = false;
  private bool _isLocked = true;

  private void Start()
  {
    LoadCharacter(characterPath);
  }

  public void CharacterReady()
  {
    _characterLoaded = true;
    _isLocked = false;
  }

  public void LoadCharacter(string path)
  {
    _characterLoaded = false;
    _isLocked = true;

    if (_character)
    {
      Destroy(_character);
    }
    _character = Instantiate(Resources.Load(characterPath + "/fpv") as GameObject, transform);
    _character.name = "fpv_" + path;
    _character.transform.localPosition = Vector3.zero;
    _character.transform.localRotation = Quaternion.identity;

    _character.GetComponent<CharacterCore>().StartUp(this);
  }

  public void SetValues(Dictionary<string, object> characterParams)
  {
    CharacterController charRb = GetComponent<CharacterController>();
    charRb.radius = (float)characterParams["width"];
    charRb.height = (float)characterParams["height"];

    maxHp = (float)characterParams["health"];
    hp = maxHp;

    PlayerMovement movement = GetComponent<PlayerMovement>();
    movement.SetJump((float)characterParams["jump"]);
    movement.SetSpeed((float)characterParams["speed"]);

    movement.CameraPos.localPosition = new Vector3(0f, (float)characterParams["camera"], 0f);

    _character.transform.parent = movement.CameraPos;

    movement.StartUp();
    GetComponent<PlayerWeapons>().StartUp();

    CharacterReady();
  }

  public bool IsLocked
  {
    get
    {
      return _isLocked;
    }
  }
}

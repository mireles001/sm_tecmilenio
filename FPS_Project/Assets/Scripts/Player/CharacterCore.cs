using UnityEngine;
using System.Collections.Generic;

public class CharacterCore : MonoBehaviour
{
  [SerializeField]
  private float _movementSpeed = 10f;
  [SerializeField]
  private float _jumpForce = 10f;
  [SerializeField]
  private float _maxHealth = 100f;
  [SerializeField]
  private float _cameraPosition = 1f;
  [SerializeField]
  private float _colliderWidth = 0.25f;
  [SerializeField]
  private float _colliderHeight = 1f;
  private PlayerCore _core;

  public void StartUp(PlayerCore core)
  {
    _core = core;

    Dictionary<string, object> characterParams = new Dictionary<string, object>();

    characterParams["speed"] = _movementSpeed;
    characterParams["jump"] = _jumpForce;
    characterParams["health"] = _maxHealth;
    characterParams["camera"] = _cameraPosition;
    characterParams["width"] = _colliderWidth;
    characterParams["height"] = _colliderHeight;

    _core.SetValues(characterParams);
  }
}

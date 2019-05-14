using UnityEngine;

public class CharacterData : MonoBehaviour
{
  [SerializeField]
  private GameObject _fpv;
  [SerializeField]
  private GameObject _tpv;
  [SerializeField]
  private Texture2D _profileTexture;
  private string _name = "";

  private void Awake()
  {
    _name = _fpv.GetComponent<CharacterCore>().Name;
  }

  public string Name
  {
    get
    {
      return _name;
    }
  }
}

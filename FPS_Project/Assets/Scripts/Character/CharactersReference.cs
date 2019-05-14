using UnityEngine;

public class CharactersReference : MonoBehaviour
{
  public GameObject[] characters;

  public string Name(int index)
  {
    return characters[index].GetComponent<CharacterData>().Name;
  }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerControl : MonoBehaviour
{
  public Transform spawnContainer;
  public GameObject thirdpensonviewprefab;
  public Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();

  private void Start()
  {
    GameState.GetInstance().Init();
  }

  private void Update()
  {
    var pls = GameState.GetInstance().players;
    foreach (KeyValuePair<int, PlayerInstance> entry in pls)
    {
      if (!dict.ContainsKey(entry.Key))
      {
        dict.Add(
        entry.Key,
        (GameObject)Instantiate(
          thirdpensonviewprefab,
          new Vector3(
            entry.Value.posX,
            entry.Value.posY,
            entry.Value.posZ
          ),
          Quaternion.identity
        )
        );
        dict[entry.Key].transform.parent = spawnContainer;
      }
      dict[entry.Key].transform.position = new Vector3(
        entry.Value.posX,
        entry.Value.posY,
        entry.Value.posZ
      );
      dict[entry.Key].transform.rotation = Quaternion.Euler(0, entry.Value.rotY, 0);
      dict[entry.Key].SetActive(entry.Key != GameState.GetInstance().localPlayer.id);
    }
  }
}

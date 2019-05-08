using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerControl : MonoBehaviour
{

    public Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();

    public Transform spawnContainer;
    // Start is called before the first frame update
    void Start()
    {
        GameState.GetInstance().init();
    }

    // Update is called once per frame
    void Update()
    {
        var pls = GameState.GetInstance().players;
        foreach(KeyValuePair<int , PlayerInstance> entry in pls) {
            if (!dict.ContainsKey(entry.Key)) {
                dict.Add(
                    entry.Key,
                    (GameObject)Instantiate(
                        Resources.Load("gorogoro/3pv"),
                        entry.Value.position,
                        Quaternion.identity
                    )
                );
                dict[entry.Key].transform.parent = spawnContainer;
            }
        }
    }
}

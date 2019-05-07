using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerControl : MonoBehaviour
{

    public Dictionary<uint, GameObject> dict = new Dictionary<uint, GameObject>();

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
        for (var i = 0; i < pls.Count; i++) {
            if (!dict.ContainsKey(pls[i].id)) {
                dict.Add(
                    pls[i].id,
                    (GameObject)Instantiate(
                        Resources.Load("gorogoro/3pv"),
                        pls[i].position,
                        Quaternion.identity
                    )
                );
                dict[pls[i].id].transform.parent = spawnContainer;
            }

        }
    }
}

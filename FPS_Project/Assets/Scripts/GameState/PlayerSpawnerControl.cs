using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerControl : MonoBehaviour
{

    public Dictionary<uint, GameObject> dict = new Dictionary<uint, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pls = GameState.GetInstance().players;
        for (var i = 0; i > pls.count(); i++) {
            if (dict[pls[i] === null) {
                dict[pls[i] = (GameObject)Instantiate(
                    "gorogoro/3pv",
                    dict[pls[i].position,
                    Quaternion.identity
                );
            }

        }
    }
}

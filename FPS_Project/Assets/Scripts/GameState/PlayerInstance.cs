using UnityEngine;
[System.Serializable]
public class PlayerInstance {
    public Vector3 position = new Vector3();
    public Vector2 rotation = new Vector2();
    public int id = 1;
    public string name = "";

    public PlayerInstance(string name, int id) {
        this.id = id;
        this.name = name;
    }
}
using UnityEngine;
[System.Serializable]
public class PlayerInstance
{
  public float posX = 0;
  public float posY = 0;
  public float posZ = 0;
  public float rotX = 0;
  public float rotY = 0;
  public int id = 1;
  public string name = "";

  public PlayerInstance(string name, int id)
  {
    this.id = id;
    this.name = name;
  }
}
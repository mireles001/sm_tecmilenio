[System.Serializable]
public class Net_PlayerPushUpdate : NetMsg
{
  public Net_PlayerPushUpdate()
  {
    OP = NetOP.PlayerPushUpdate;
  }

  public float posX { set; get; }
  public float posY { set; get; }
  public float posZ { set; get; }

  public float rotX { set; get; }
  public float rotY { set; get; }

//   public float isGrounded { set; get; }
//   public float isRunning { set; get; }
//   public float isFiring { set; get; }
}

[System.Serializable]
public class Net_PlayerPushUpdate : NetMsg
{
  public Net_PlayerPushUpdate()
  {
    OP = NetOP.PlayerPushUpdate;
  }

  public PlayerInstance player;
}

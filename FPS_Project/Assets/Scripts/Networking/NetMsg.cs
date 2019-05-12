public static class NetOP
{
  public const int None = 0;
  public const int SetUsername = 1;
  public const int SetConnectionId = 2;
  public const int PlayerPushUpdate = 3;
  public const int GameState = 4;
}

[System.Serializable]
public abstract class NetMsg
{
  public byte OP { set; get; } // Operation Code

  public NetMsg()
  {
    OP = NetOP.None;
  }
}

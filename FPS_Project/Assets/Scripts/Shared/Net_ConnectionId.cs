[System.Serializable]
public class Net_ConnectionId : NetMsg
{
  public Net_ConnectionId()
  {
    OP = NetOP.SetConnectionId;
  }

  public int ConnectionId { set; get; }
}

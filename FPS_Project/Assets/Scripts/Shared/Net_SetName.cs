[System.Serializable]
public class Net_SetUsername : NetMsg
{
  public Net_SetUsername()
  {
    OP = NetOP.SetUsername;
  }

  public string Username { set; get; }
}

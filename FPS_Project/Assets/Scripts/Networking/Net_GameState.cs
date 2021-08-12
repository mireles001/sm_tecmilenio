using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Net_GameState : NetMsg
{
  public Net_GameState()
  {
    OP = NetOP.GameState;
  }

  public List<PlayerInstance> players;

}

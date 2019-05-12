using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Net_GameState : NetMsg
{
  public Net_GameState()
  {
    OP = NetOP.GameState;
  }

  List<float> a = new List<float>();

  //   public float isGrounded { set; get; }
  //   public float isRunning { set; get; }
  //   public float isFiring { set; get; }
}

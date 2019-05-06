using System;
using System.Collections;
using System.Collections.Generic;

public class GameState {
    public List<PlayerInstance> players = new List<PlayerInstance>(); 
    
    #region Singleton Implementation
    private static object locker = new object();
    private static GameState _instance = null;

    private GameState()
    { 
    }
    
    public static GameState GetInstance()
    {
        lock (locker)
        {
            if (_instance == null)
            {
                _instance = new GameState();
            }
            return _instance;
        }
    }
  #endregion

  public void init() {
    players = new List<PlayerInstance>(); 
    var pl = new PlayerInstance();
    pl.id = 2;
    players.Add(pl);
  }
    

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState {
    // public List<PlayerInstance> players = new List<PlayerInstance>();
    public Dictionary<int, PlayerInstance> players;
    public PlayerInstance localPlayer = new PlayerInstance("", 0);
    
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
    // players = new List<PlayerInstance>();
    players = new Dictionary<int, PlayerInstance>();
    // var pl = new PlayerInstance();
    // pl.id = 2;
    // players.Add(pl);
  }

  public void addPlayer(PlayerInstance player) {
    players.Add(player.id, player);
  }
    
  public void updatePlayer(
      int id,
      float posX,
      float posY,
      float posZ
  ) {
    if (players.ContainsKey(id)) {
        players[id].position = new Vector3(posX, posY, posZ);
    } else {
        Debug.Log("player not created " + id);
    }
  }

}
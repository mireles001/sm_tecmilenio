using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState {
    // public List<PlayerInstance> players = new List<PlayerInstance>();
    public Dictionary<int, PlayerInstance> players;
    public PlayerInstance localPlayer = new PlayerInstance("", 0);

  private List<PlayerInstance> _playerList;

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
    players = new Dictionary<int, PlayerInstance>();
    _playerList = new List<PlayerInstance>();
  }

  public void addPlayer(PlayerInstance player) {
    players.Add(player.id, player);
    _playerList.Add(player);
  }

  public void removePlayer()
  {

  }

  public List<PlayerInstance> getPlayerList()
  {
    return _playerList;
  }
    
  public void updatePlayer(
    PlayerInstance player
  ) {
    if (players.ContainsKey(player.id)) {
      players[player.id] = player;
    } else {
      Debug.Log("player not created " + player.id);
    }
  }

  public void updateState(List<PlayerInstance> newList)
  {
    for (int i = 0; i < newList.Count; i++)
    {
      players[newList[i].id] = newList[i];
    }
  }

}
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
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

  public void Init()
  {
    players = new Dictionary<int, PlayerInstance>();
  }

  public void AddPlayer(PlayerInstance player)
  {
    players.Add(player.id, player);
  }

  public void RemovePlayer()
  {

  }

  public List<PlayerInstance> GetPlayerList()
  {
    List<PlayerInstance> playerList = new List<PlayerInstance>();
    foreach (KeyValuePair<int, PlayerInstance> entry in players)
    {
      playerList.Add(entry.Value);
    }
    return playerList;
  }

  public void UpdatePlayer(PlayerInstance player)
  {
    Debug.Log("Updating from client " + player.id);
    if (players.ContainsKey(player.id))
    {
      players[player.id] = player;
    }
    else
    {
      Debug.Log("Player not created " + player.id);
    }
  }

  public void UpdateState(List<PlayerInstance> newList)
  {
    Debug.Log("Updating from server. mi id is: " + localPlayer.id);
    for (int i = 0; i < newList.Count; i++)
    {
      Debug.Log("id " + newList[i].id);
      players[newList[i].id] = newList[i];
    }
  }

}
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, ISelectHandler
{
  private int _index;
  private GameManager _manager;

  public void StartUp(int index, GameManager manager)
  {
    _index = index;
    _manager = manager;
  }

  public void OnSelect(BaseEventData eventData)
  {
    _manager.SelectionCursor =  _index;
  }
}

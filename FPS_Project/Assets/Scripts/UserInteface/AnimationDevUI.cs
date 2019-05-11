using UnityEngine;
using UnityEngine.UI;

public class AnimationDevUI : MonoBehaviour
{
  public GameObject characterPrefab;
  public Button use;
  public Button mg;
  public Button gl;
  public Button rl;
  public Button run;
  public Button runBack;
  public Button idle;
  public Button jump;
  public Button land;
  public Button damage;
  public Button death;
  public Button reset;

  private GameObject _character;
  private TpvAnimation _tpv;

  private void Start()
  {
    CreateChar();
    reset.onClick.AddListener(Reset);
  }

  private void CreateChar()
  {
    if (_character)
    {
      Destroy(_character);
      _tpv = null;
    }

    _character = Instantiate(characterPrefab);
    _character.transform.SetPositionAndRotation(new Vector3(-1.7f, 0f, 1f), Quaternion.Euler(new Vector3(0f, -180f, 0f)));

    _tpv = _character.GetComponent<TpvAnimation>();

    use.onClick.AddListener(delegate { _tpv.WeaponUse(); });
    mg.onClick.AddListener(delegate { WeaponChange(1); });
    gl.onClick.AddListener(delegate { WeaponChange(2); });
    rl.onClick.AddListener(delegate { WeaponChange(3); });
    run.onClick.AddListener(delegate { Run(1); });
    runBack.onClick.AddListener(delegate { Run(-1); });
    idle.onClick.AddListener(Idle);
    jump.onClick.AddListener(delegate { _tpv.Jump(); });
    land.onClick.AddListener(SetIsGrounded);
    damage.onClick.AddListener(delegate { _tpv.Damage(); });
    death.onClick.AddListener(delegate { _tpv.Death(); });

    SetIsGrounded();
    WeaponChange(1);
  }

  private void WeaponChange(int wIndex)
  {
    _tpv.WeaponChange(wIndex);
  }

  private void SetIsGrounded()
  {
    _tpv.SetIsGrounded(true);
  }

  private void Run(float val)
  {
    _tpv.SetIsRunning(true);
    _tpv.SetMovement(val);
  }

  private void Idle()
  {
    SetIsGrounded();
    _tpv.SetIsRunning(false);
  }

  private void Reset()
  {
    CreateChar();
  }
}

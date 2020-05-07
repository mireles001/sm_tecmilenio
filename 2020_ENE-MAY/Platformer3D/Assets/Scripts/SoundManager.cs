using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public AudioClip button;
  public AudioClip jump;
  public AudioClip land;
  public AudioClip death;
  public AudioClip win;
  public AudioClip pickup;
  private AudioSource _source;

  private void Start()
  {
    _source = GetComponent<AudioSource>();
  }

  public void SfxButton()
  {
    if (_source && button) _source.PlayOneShot(button);
  }

  public void SfxJump()
  {
    if (_source && jump) _source.PlayOneShot(jump);
  }

  public void SfxLand()
  {
    if (_source && land) _source.PlayOneShot(land);
  }

  public void SfxDeath()
  {
    if (_source && death) _source.PlayOneShot(death);
  }

  public void SfxWin()
  {
    if (_source && win) _source.PlayOneShot(win);
  }

  public void SfxPickup()
  {
    if (_source && pickup) _source.PlayOneShot(pickup);
  }
}

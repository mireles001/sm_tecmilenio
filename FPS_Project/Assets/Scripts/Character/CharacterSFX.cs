using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
  public AudioClip jump;
  public AudioClip land;
  public AudioClip weaponSwap;
  public AudioClip machineGun;
  public AudioClip grenade;
  public AudioClip rocket;
  public AudioClip damage;
  public AudioClip death;
  public AudioClip health;
  public AudioClip ammo;
  private AudioSource _source;

  private void Awake()
  {
    _source = transform.parent.gameObject.AddComponent<AudioSource>();
    _source.playOnAwake = false;
  }

  private void PlaySound(AudioClip clip)
  {
    _source.PlayOneShot(clip);
  }

  public void Jump()
  {
    PlaySound(jump);
  }

  public void Land()
  {
    PlaySound(land);
  }

  public void WeaponSwap()
  {
    PlaySound(weaponSwap);
  }

  public void FireGun()
  {
    PlaySound(machineGun);
  }

  public void FireGrenade()
  {
    PlaySound(grenade);
  }

  public void FireRocket()
  {
    PlaySound(rocket);
  }

  public void Death()
  {
    PlaySound(death);
  }

  public void PickHealth()
  {
    PlaySound(health);
  }

  public void PickAmmo()
  {
    PlaySound(ammo);
  }

  public void Damage()
  {
    PlaySound(damage);
  }
}

using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
  [SerializeField]
  private AudioClip _jump;
  [SerializeField]
  private AudioClip _land;
  [SerializeField]
  private AudioClip _weaponSwap;
  [SerializeField]
  private AudioClip _machineGun;
  [SerializeField]
  private AudioClip _grenade;
  [SerializeField]
  private AudioClip _rocket;
  [SerializeField]
  private AudioClip _damage;
  [SerializeField]
  private AudioClip _death;
  [SerializeField]
  private AudioClip _health;
  [SerializeField]
  private AudioClip _ammo;
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
    PlaySound(_jump);
  }

  public void Land()
  {
    PlaySound(_land);
  }

  public void WeaponSwap()
  {
    PlaySound(_weaponSwap);
  }

  public void FireGun()
  {
    PlaySound(_machineGun);
  }

  public void FireGrenade()
  {
    PlaySound(_grenade);
  }

  public void FireRocket()
  {
    PlaySound(_rocket);
  }

  public void Death()
  {
    PlaySound(_death);
  }

  public void PickHealth()
  {
    PlaySound(_health);
  }

  public void PickAmmo()
  {
    PlaySound(_ammo);
  }

  public void Damage()
  {
    PlaySound(_damage);
  }
}

using UnityEngine;
using Player;

public class CharacterFx : MonoBehaviour, ICanJump
{
    [SerializeField]
    private ParticleSystem _landingParticles = null;

    private void Start()
    {
        GetComponent<PlayerMovement>().RegisterJump(this);
    }

    public void Jump()
    {
        //Debug.Log("Jumping FX");
    }

    public void Landing(float velocity)
    {
        if (velocity > -2) return;

        Transform particle = Instantiate(_landingParticles).transform;
        particle.SetPositionAndRotation(transform.position, transform.rotation);

    }

    public string ID { get { return GetInstanceID().ToString(); } }
}

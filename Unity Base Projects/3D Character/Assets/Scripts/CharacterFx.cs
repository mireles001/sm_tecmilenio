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

    public void Landing()
    {
        //Debug.Log("Landing FX");
    }

    public string ID { get { return GetInstanceID().ToString(); } }
}

using UnityEngine;

public class SelfDestroyParticleEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private bool effectPlayed = false;

    void Start()
    {
        effect = gameObject.GetComponent<ParticleSystem>();
        effect.Play();
    }

    void Update()
    {
        if (!effect.isPlaying)
        {
            Destroy(gameObject);
        }

    }
}
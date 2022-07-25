using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    public float volume = 0.5f;

    private AudioSource characterAudio;

    [SerializeField]
    private AudioClip getHitSound;
    [SerializeField]
    private AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        characterAudio = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        characterAudio.PlayOneShot(getHitSound);
    }
    public void PlayDeathSound()
    {
        characterAudio.PlayOneShot(deathSound);
    }
}

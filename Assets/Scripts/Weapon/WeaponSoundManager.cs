using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundManager : MonoBehaviour
{
    private AudioSource objectAudio;

    public AudioClip[] fallingWeaponSound;
    public AudioClip[] weaponHitObstacleSound;
    public AudioClip[] weaponAttackSound;
    public AudioClip[] swordHitShieldSound;    

    public float volume = 0.5f;

    private float maxSoundDistance = 80f;
    // Start is called before the first frame update
    void Awake()
    {
        objectAudio = gameObject.AddComponent<AudioSource>();
        objectAudio.maxDistance = maxSoundDistance;
    }
    public void PlayWeaponSwingSound()
    {

    }
    public void PlayFallingWeaponSound()
    {
        int index = Random.Range(0, fallingWeaponSound.Length);
        if (fallingWeaponSound.Length != 0)
            objectAudio.PlayOneShot(fallingWeaponSound[index], volume);
    }
    public void PlayWeaponHitObstacleSound()
    {
        int index = Random.Range(0, weaponHitObstacleSound.Length);
        if (weaponHitObstacleSound.Length != 0)
            objectAudio.PlayOneShot(weaponHitObstacleSound[index], volume);
    }
    public void PlayWeaponAttackSound()
    {
        int index = Random.Range(0, weaponAttackSound.Length);        
        if (weaponAttackSound.Length != 0) 
            objectAudio.PlayOneShot(weaponAttackSound[index], volume);            
    }

    public void PlayWeaponHitShieldSound()
    {
        int index = Random.Range(0, swordHitShieldSound.Length);
        if (swordHitShieldSound.Length != 0)
            objectAudio.PlayOneShot(swordHitShieldSound[index], volume);
    }
}

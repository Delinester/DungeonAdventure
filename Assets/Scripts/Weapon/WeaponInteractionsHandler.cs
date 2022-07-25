using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteractionsHandler : MonoBehaviour
{
    private WeaponSoundManager soundManager;    
    private WeaponDataStorage weaponData;

    private bool wasAttackMade;
    // Start is called before the first frame update
    void Awake()
    {
        soundManager = GetComponent<WeaponSoundManager>();
        weaponData = GetComponent<WeaponDataStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterBase characterController = GetComponentInParent<CharacterBase>();
        TrailRenderer weaponTrail = GetComponentInChildren<TrailRenderer>();

        if (characterController != null && !characterController.isAttacking) wasAttackMade = false;

        if (characterController != null && characterController.isAttacking && weaponTrail != null)
        {
            weaponTrail.emitting = true;
            weaponTrail.enabled = true;
        }
        else if (weaponTrail != null)
        {            
            weaponTrail.emitting = false;
            weaponTrail.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {            
            if (weaponData.attachment == WeaponDataStorage.WeaponAttachment.Nobody)
            {
                soundManager.PlayFallingWeaponSound();
            }
        else if (collision.collider.CompareTag("Obstacle") && weaponData.attachment != WeaponDataStorage.WeaponAttachment.Nobody && 
                weaponData.GetWeaponOwner().GetComponent<CharacterBase>().isAttacking)
            {
                soundManager.PlayWeaponHitObstacleSound();
            }
        }
        bool weaponIsAttachedToSomebody = weaponData != null && weaponData.attachment != WeaponDataStorage.WeaponAttachment.Nobody ? true : false;        
        bool isCollisionCharacterBased = collision.gameObject.TryGetComponent<CharacterBase>(out CharacterBase collisionCharacter) ? true : false;

        if (weaponIsAttachedToSomebody && isCollisionCharacterBased &&
            transform.root.GetComponentInChildren<CharacterBase>().isAttacking &&             
            transform.root.GetComponentInChildren<CharacterBase>().gameObject.tag != collisionCharacter.gameObject.tag)
        {
            if (!collisionCharacter.gameObject.GetComponent<CharacterBase>().isDefending && !wasAttackMade)
            {
                wasAttackMade = true;
                collisionCharacter.ReceiveDamage(weaponData.GetDamage());                
            }
            else if (collisionCharacter.gameObject.GetComponent<CharacterBase>().isDefending)
            {
                soundManager.PlayWeaponHitShieldSound();
                float damageBlock = collisionCharacter.GetComponent<WeaponManager>().GetCurrentShield().GetComponent<WeaponDataStorage>().GetDamageBlock();
                float staminaToSpend = collisionCharacter.GetComponent<WeaponManager>().GetCurrentShield().GetComponent<WeaponDataStorage>().GetStaminaCost();
                collisionCharacter.GetComponent<CharacterBase>().SpendStamina(staminaToSpend);
                collisionCharacter.ReceiveDamage((int)(weaponData.GetDamage() * damageBlock / 100));
            }
        }
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerEnemy : WeaponManager
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon") &&
            collision.gameObject.GetComponentInChildren<WeaponDataStorage>().attachment == WeaponDataStorage.WeaponAttachment.Nobody)
        {
            WeaponDataStorage weaponData = weaponObject.GetComponent<WeaponDataStorage>();

            if (weaponObject != null && weaponData.canBeDropped &&
                collision.gameObject.GetComponent<WeaponDataStorage>().GetDamage() > weaponData.GetDamage())
            {                
                DropWeapon();
                weaponObject = collision.gameObject;
                weaponObject.GetComponent<Rigidbody>().isKinematic = false; //true
            }
            else if (!weaponObject.GetComponent<WeaponDataStorage>().canBeDropped && weaponData.canBeReplaced)
            {                
                Destroy(weaponObject);
                weaponObject = collision.gameObject;
                weaponObject.GetComponent<Rigidbody>().isKinematic = false; //true
            }

        }
        if (collision.gameObject.CompareTag("Shield") && collision.transform.root.GetComponentInChildren<CharacterBase>() == null &&
            shieldPlacement != null)
        {
            shieldObject = collision.gameObject;
            //weaponObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}

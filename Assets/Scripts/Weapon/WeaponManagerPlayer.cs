using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerPlayer : WeaponManager
{
    public void PickUp(GameObject obj)
    {
        if (obj.CompareTag("Weapon") && obj.GetComponentInChildren<WeaponDataStorage>().attachment == WeaponDataStorage.WeaponAttachment.Nobody)
        {
            if (weaponObject != null) DropWeapon();
            weaponObject = obj;
            weaponObject.GetComponent<Rigidbody>().isKinematic = false; //true            
        }
        if (obj.CompareTag("Shield"))
        {
            shieldObject = obj.gameObject;
            //weaponObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

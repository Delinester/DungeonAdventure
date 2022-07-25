using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weaponPlacement;
    public GameObject shieldPlacement;
    public GameObject weaponPrefab;

    protected GameObject weaponObject = null;
    protected GameObject shieldObject = null;
    protected Vector3 dropDistance = new Vector3(5f, 2f, 5f);    
    
    
    private void Start()
    {
        if (weaponPrefab != null)
        {
            weaponObject = Instantiate(weaponPrefab);
        }
    }
    protected void Update()
    {
        if (weaponObject != null)
        {
            CheckWeaponAttachment();
            if (weaponObject.transform.parent != weaponPlacement.transform)
            {
                weaponObject.transform.parent = weaponPlacement.transform;
            }
            weaponObject.transform.position = weaponPlacement.transform.position;
            weaponObject.transform.rotation = weaponPlacement.transform.rotation;
        }
        if (shieldObject != null)
        {
            CheckShieldAttachment();
            if (shieldObject.transform.parent != shieldPlacement.transform)
            {
                shieldObject.transform.parent = shieldPlacement.transform;
            }
            shieldObject.transform.position = shieldPlacement.transform.position;
            shieldObject.transform.rotation =
                Quaternion.Euler(shieldPlacement.transform.rotation.eulerAngles.x + 90, shieldPlacement.transform.rotation.eulerAngles.y, 0);
        }
    }
    protected void CheckWeaponAttachment()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            weaponObject.GetComponent<WeaponDataStorage>().attachment = WeaponDataStorage.WeaponAttachment.Enemy;
        }
        else if (gameObject.CompareTag("Player"))
        {
            weaponObject.GetComponent<WeaponDataStorage>().attachment = WeaponDataStorage.WeaponAttachment.Player;
        }
    }
    protected void CheckShieldAttachment()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            shieldObject.GetComponent<WeaponDataStorage>().attachment = WeaponDataStorage.WeaponAttachment.Enemy;
        }
        else if (gameObject.CompareTag("Player"))
        {
            shieldObject.GetComponent<WeaponDataStorage>().attachment = WeaponDataStorage.WeaponAttachment.Player;
        }
    }
    public GameObject GetCurrentWeapon()
    {
        return weaponObject;
    }
    public GameObject GetCurrentShield() => shieldObject;
    
    public void DropWeapon()
    {
        if (weaponObject.GetComponent<WeaponDataStorage>().canBeDropped)
        {
            Debug.Log("DROPPING INITIATED");
            Rigidbody weaponRb = weaponObject.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;            
            weaponObject.transform.position = gameObject.transform.position + dropDistance;
            weaponObject.GetComponent<WeaponDataStorage>().attachment = WeaponDataStorage.WeaponAttachment.Nobody;

            if (HasWeapon()) weaponObject.transform.parent = null;
            if (HasShield()) shieldObject.transform.parent = null;
        }
        else
        {
            Destroy(weaponObject);
        }
    }

    public bool HasWeapon()
    {
        return weaponObject == null ? false : true;
    }
    public bool HasShield()
    {
        return shieldObject == null ? false : true;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataStorage : MonoBehaviour
{        
    [SerializeField]
    private WeaponData data;

    public WeaponAttachment attachment;
    private string weaponName;
    private float damageBlockPercent;
    private int damage;
    private float staminaCost;
    private float cost;

    public bool canBeDropped;
    public bool canBeReplaced = true;

    private GameObject weaponOwner;
    public enum WeaponAttachment
    {
        Player,
        Enemy,
        Nobody
    }
    void Start()
    {
        weaponName = data.weaponName;
        damageBlockPercent = data.damageBlockPercent;
        damage = data.damage;
        staminaCost = data.staminaCost;
        cost = data.cost;
    }
    private void Update()
    {
        if (attachment != WeaponAttachment.Nobody)
        {
            weaponOwner = GetComponentInParent<CharacterBase>().gameObject;
        }
    }
    public string GetName() => weaponName;
    public float GetDamageBlock() => damageBlockPercent;
    public int GetDamage() => damage;
    public float GetStaminaCost() => staminaCost;
    public float GetCost() => cost;
    public GameObject GetWeaponOwner() => weaponOwner;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{    
    [SerializeField]
    protected int health = 100;
    [HideInInspector]
    public BaseAnimator characterAnimator;

    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool isDefending;
    [HideInInspector]
    public bool isAlive = true;
    protected void CheckAnimationState()
    {
        if (characterAnimator.GetCurrentAnimation().IsTag("Attack"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    public virtual void SpendStamina(float staminaToSpend)
    {

    }
    public virtual void ReceiveDamage(int damage)
    {
        health -= damage;
        characterAnimator.PerformHitAnim();
        GetComponent<CharacterSoundManager>().PlayHitSound();
    }
    public int GetCurrentHealthPoints()
    {
        return health;
    }
    public void SetHealthTo(int healthPoints)
    {
        health = healthPoints;
    }
}

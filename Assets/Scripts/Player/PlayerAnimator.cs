using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : BaseAnimator
{    
    private PlayerController playerController;

    // Start is called before the first frame update
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isDefending_b", playerController.isDefending);
        animator.SetFloat("Speed_f", playerController.GetVerticalInput() + playerController.GetSprintValue());        
        animator.SetFloat("horizontalInput_f", playerController.GetHorizontalInput());
        animator.SetBool("isIdle_b", playerController.IsStanding());       
    }

    public void PerformAttackAnim()
    {
        int index = Random.Range(0, 3);
        animator.SetTrigger("Attack_trig");
        animator.SetInteger("AttackType_int", index);
    }  
}

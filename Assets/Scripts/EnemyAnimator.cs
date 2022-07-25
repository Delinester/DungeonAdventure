using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : BaseAnimator
{    
    private Enemy enemy;
    private NavMeshAgent agent;
    [SerializeField]
    private int numberOfAttackAnims = 3;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed_f", agent.velocity.magnitude / agent.speed);        
        animator.SetBool("isIdle_b", agent.velocity.magnitude == 0 ? true : false);        
    }

    public void PerformAttackAnim()
    {
        int index = Random.Range(0, numberOfAttackAnims);
        animator.SetInteger("AttackType_int", index);
        animator.SetTrigger("Attack_trig");        
    }
    public void PerformDeadAnim()
    {
        animator.SetTrigger("die_trig");
    }
}

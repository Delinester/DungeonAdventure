using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimator : MonoBehaviour
{
    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    

    public void PerformHitAnim()
    {
        animator.SetTrigger("hit_trig");
    }

    public AnimatorStateInfo GetCurrentAnimation()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }
}

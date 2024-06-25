using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PetAnimator : MonoBehaviour
{
    [SerializeField] string IdleKeyword;
    [SerializeField] string RestKeyword;
    [SerializeField] string SickKeyword;
    [SerializeField] string DeathKeyword;
    [SerializeField] string CheerKeyword;
    [SerializeField] string EatKeyword;
    [SerializeField] string ReactKeyword;
    [SerializeField] string ReviveKeyword;

    Animator animator;

    public void SetupAniamtor(Animator animator)
    {
        this.animator = animator;
    }

    public void ResetAnimator()
    {
        animator.SetInteger(IdleKeyword, 1);
        animator.SetBool(RestKeyword, false);
        animator.SetBool(DeathKeyword, false);
        animator.SetBool(SickKeyword, false);
    }

    public void PlayIdleAnimation(int animNum)
    {
        ResetAnimator();
        animator.SetInteger(IdleKeyword, animNum);
    }

    public void PlaySleepAnimation()
    {
        ResetAnimator();
        animator.SetBool(RestKeyword, true);
    }

    public void PlaySickAnimation()
    {
        ResetAnimator();
        animator.SetBool(SickKeyword, true);
    }

    public void PlayDeathAnimation()
    {
        
        ResetAnimator();
        animator.SetTrigger(DeathKeyword);
    }

    public void PlayCheeringAnimation()
    {
        ResetAnimator();
        animator.SetTrigger(CheerKeyword);
    }

    public void PlayEatingAnimation()
    {
        ResetAnimator();
        animator.SetTrigger(EatKeyword);
    }

    public void PlayReactAnimation()
    {
        ResetAnimator();
        animator.SetTrigger(ReactKeyword);
    }

    public void PlayReviveAnimation()
    {
        ResetAnimator();
        animator.SetTrigger(ReviveKeyword);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartApperAnim()
    {
        animator.SetTrigger("Appear");
        //animator.SetTrigger("Dissolve");
    }

    public void StartInteract()
    {
        animator.SetTrigger("Interact");
    }

    public void StopInteract()
    {
        animator.SetBool("Close", true);
    }

    public void CloseInteractable()
    {
        animator.SetBool("Close", false);
    }

    public void ChooseInteractable(int num)
    {
        animator.SetInteger("Innum", num);
    }

}

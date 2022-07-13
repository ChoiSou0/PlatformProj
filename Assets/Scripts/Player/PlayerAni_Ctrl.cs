using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni_Ctrl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    public void ExitDash()
    {
        animator.SetBool("isDash", false);
    }

    public void ExitATK_Ani()
    {
        animator.SetBool("isATK1", false);
        animator.SetBool("isATK2", false);
    }
}

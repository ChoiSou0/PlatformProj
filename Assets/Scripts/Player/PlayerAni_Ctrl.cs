using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni_Ctrl : Player_Ctrl
{
    public void ExitATK_Ani()
    {
        animator.SetBool("isATK1", false);
        animator.SetBool("isATK2", false);
    }
}

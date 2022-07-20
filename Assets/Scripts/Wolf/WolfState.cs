using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WolfState : Wolf_Ctrl
{
    public abstract void Action(WolfAni wolfAni);
}

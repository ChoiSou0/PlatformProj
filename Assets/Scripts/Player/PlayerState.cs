using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void Action(Player_Ctrl.PlayerState playerState);
};


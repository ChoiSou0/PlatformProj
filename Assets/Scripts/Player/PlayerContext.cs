using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    private State state;

    public PlayerContext(State state)
    {
        this.state = state;
    }

    public void SetState(State state)
    {
        this.state = state;
    }

    public void Act()
    {
        //state.Action();
    }

}

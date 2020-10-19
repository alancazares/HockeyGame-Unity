using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPuck : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("WaitingOffence", 1);
        return true;
    }
}

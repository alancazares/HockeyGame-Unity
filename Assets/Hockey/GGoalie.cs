﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGoalie : GAgent
{
    void Start()
    {

        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);
    }

}

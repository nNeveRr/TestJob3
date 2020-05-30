using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using pathFinding;

public class BlueEnemy : Enemy
{
    protected override void StateMainMoving()
    {

        if (PathFinished || StepPathDone == 2)
        {
            PathFinished = false;
            CalculatePathToPlayer();
        }
        else
        {
            MoveRelease();
        }
    }
}

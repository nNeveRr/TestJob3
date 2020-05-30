using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using pathFinding;

public class WhiteEnemy : Enemy
{

    protected override void  StateMainMoving()
    {
        if (GetDistanceBetweenPlayer() < 2f)
        {
            PathFinished = true;
            CurrentState = StateFollowPlayer;
        }
        else

        if (PathFinished)
        {
            PathFinished = false;
            CalculateRandomPath();
        }
        else
        {
            MoveRelease();
        }
    }

    void StateFollowPlayer()
    {
        if (GetDistanceBetweenPlayer() < 4f)
        {
            if(StepPathDone == 2 || PathFinished)
            {
                PathFinished = false;
                CalculatePathToPlayer();
            }
            MoveRelease();
        }
        else
        {
            CurrentState = StateMainMoving;
        }
    }
}

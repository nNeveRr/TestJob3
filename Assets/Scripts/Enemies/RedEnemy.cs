using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using pathFinding;

public class RedEnemy : Enemy
{
    Vector3 DeltaPlayerMove;
    Vector3 LastPlayerPos;

    protected override void StateMainMoving()
    {
        if (GetDistanceBetweenPlayer() < 2f)
        {
            PathFinished = true;
            CurrentState = StateFollowPlayer;
        }
        else

        if (PathFinished || StepPathDone == 10)
        {
            PathFinished = false;
            CalculateAmbushPath();
        }
        else
        {
            WatchPlayer();
            MoveRelease();
        }
    }

    void WatchPlayer()
    {
        Vector3 playerPos = MapController.GetWoldPlayerPosition();
        DeltaPlayerMove = playerPos - LastPlayerPos;
        LastPlayerPos = playerPos;
    }

    void StateFollowPlayer()
    {
        if (GetDistanceBetweenPlayer() < 4f)
        {
            if (StepPathDone == 2 || PathFinished)
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


    void CalculateAmbushPath()
    {
        StepPathDone = 0;
        CurrentPath = null;
        int AntiCycle = 0;
        Vector2Int PlayerPos = MapController.GetGridPlayerPosition();
        List<Vector2Int> Exclude = new List<Vector2Int>();

        Exclude.Add(PlayerPos);
        while (CurrentPath == null)
        {
            Vector2Int Prognosed = new Vector2Int((int)Mathf.Sign(DeltaPlayerMove.x),(int) Mathf.Sign(DeltaPlayerMove.z));
            Prognosed *= -3;
            Vector2Int To = PlayerPos + (AntiCycle>10? Vector2Int.zero : Prognosed) + new Vector2Int(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
            Vector2Int from = MapController.GetInGridPos(transform.position);
            CurrentPath = PathFinder.CalculatePath(from, To, Exclude);

            AntiCycle++;

            if(AntiCycle>25)
            {
                StartWait(1f);
                return;
            }

        }
    }

}

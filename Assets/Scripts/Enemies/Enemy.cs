using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pathFinding;
public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float Delay = 2f;

    protected float MoveSpeed = 1f;
    protected List<PathNode> CurrentPath;

    protected Action CurrentState;

    protected bool PathFinished = true;

    //флаг, чтобы до конца прохода 2 тайлов враг не менял направление, а то слишком часто получается
    protected int StepPathDone = 0;


    void Start()
    {
        CurrentState = StateStartWait;
    }

    void FixedUpdate()
    {
        if (GameManager.isGameStarted)
        {
            CurrentState();
        }
    }

    void StateStartWait()
    {
        Delay -= Time.deltaTime;

        if (Delay < 0f)
        {
            CurrentState = StateMainMoving;
        }
    }

    protected virtual void StateMainMoving()
    {

    }

    protected void StartWait(float Time)
    {
        Delay = Time;
        CurrentState = StateStartWait;
    }
    protected void MoveRelease()
    {
        if(CurrentPath==null)
        {
            StartWait(1f);
            return;
        }
        Vector3 moveTo = MapController.GetWoldPosFromGrid(CurrentPath[0].Tile);

        transform.position = Vector3.MoveTowards(transform.position, moveTo, MoveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, moveTo) < 0.05f)
        {
            StepPathDone++;
            CurrentPath.Remove(CurrentPath[0]);
        }

        if (CurrentPath.Count == 0)
        {
            PathFinished = true;
        }
    }


    protected void CalculateRandomPath()
    {
        StepPathDone = 0;
        Vector2Int rand = MapController.GetWoldRandomPoint();
        Vector2Int from = MapController.GetInGridPos(transform.position);
        CurrentPath = PathFinder.CalculatePath(from, rand);
    }

    protected void CalculatePathToPlayer()
    {
        StepPathDone = 0;
        Vector2Int To = MapController.GetGridPlayerPosition();
        Vector2Int from = MapController.GetInGridPos(transform.position);
        CurrentPath = PathFinder.CalculatePath(from, To);
    }

    protected float GetDistanceBetweenPlayer()
    {
        Vector3 playerPos = MapController.GetWoldPlayerPosition();
        return Vector3.Distance(transform.position, playerPos);
    }
}

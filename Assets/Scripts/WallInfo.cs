using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//часть логики, чтобы визуально собрать лабиринт. 
public class WallInfo : MonoBehaviour
{
    public Vector2Int pos;
    public MapTileObjectType myType;

    //private void OnMouseDown()
    //{
    //    MapController.Instance.PostDestroyWall(pos, myType);
    //    Destroy(gameObject);
    //}
}

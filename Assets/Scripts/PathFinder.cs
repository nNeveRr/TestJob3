using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace pathFinding
{
    public static class PathFinder
    {
        static List<PathNode> Watched = new List<PathNode>();
        static List<Vector2Int> ExcludePoints = new List<Vector2Int>();
        static PathNode From;
        static PathNode To;

        static int[,] CurrentGrid;

        public static List<PathNode> CalculatePath(Vector2Int from, Vector2Int to, List<Vector2Int> excludePoints)
        {
            ExcludePoints = excludePoints;
            List<PathNode> Result = CalculatePath(from, to);
            ExcludePoints = new List<Vector2Int>();
            return Result;
        }

        public static List<PathNode> CalculatePath(Vector2Int from, Vector2Int to)
        {
            Watched.Clear();
            CurrentGrid = MapController.GetCurrentGrid();

            From = new PathNode(from);
            To = new PathNode(to);

            List<PathNode> CurrentWatch = GetToWatch(To);
            List<PathNode> NextWatch = new List<PathNode>();

            while (true)
            {
                if (CurrentWatch.Count == 0)
                {
                    Debug.Log("Path Not Found!!");
                    Debug.Log("From : " + From.Tile.ToString());
                    Debug.Log("To : " + To.Tile.ToString());
                    return null;
                }
                foreach (var i in CurrentWatch)
                {
                    if (i.Tile == From.Tile)
                    {
                        From.Cost = i.Cost;
                        Debug.Log("Path Found!!");
                        Debug.Log("From : " + From.Tile.ToString());
                        Debug.Log("To : " + To.Tile.ToString());
                        return GetFoundedPath();
                    }
                    Watched.Add(i);
                }

                foreach (var i in CurrentWatch)
                {
                    NextWatch.AddRange(GetToWatch(i));
                }

                CurrentWatch = new List<PathNode>(NextWatch);

                NextWatch.Clear();
            }

        }

        static List<PathNode> GetFoundedPath()
        {
            PathNode Current = From;
            int CurrentCost = Current.Cost;
            List<PathNode> Result = new List<PathNode>();

            Result.Add(From);

            while (CurrentCost > 1)
            {
                Result.Add(GetNextFounded(Current));
                Current = Result.Last();
                CurrentCost--;
            }
            Result.Add(To);
            return Result;
        }

        static PathNode GetNextFounded(PathNode Current)
        {
            List<PathNode> result = new List<PathNode>();
            PathNode cand;
            List<Vector2Int> Dir = new List<Vector2Int>();
            Dir.Add(new Vector2Int(1, 0));
            Dir.Add(new Vector2Int(0, 1));
            Dir.Add(new Vector2Int(-1, 0));
            Dir.Add(new Vector2Int(0, -1));

            foreach (var d in Dir)
            {
                cand = Watched.Where(t => t.Tile == Current.Tile + d && t.Cost == Current.Cost - 1).FirstOrDefault();

                if (cand != null && isWallValid(Current.Tile, d)&&!isExcludePoint(Current.Tile))
                {
                    result.Add(cand);
                    Watched.Remove(cand);
                }
            }
            return result[UnityEngine.Random.Range(0, result.Count)];
        }
        static List<PathNode> GetToWatch(PathNode cand)
        {


            List<PathNode> Result = new List<PathNode>();
            PathNode ResCand;
            List<Vector2Int> Dir = new List<Vector2Int>();


            Dir.Add(new Vector2Int(1, 0));
            Dir.Add(new Vector2Int(0, 1));
            Dir.Add(new Vector2Int(-1, 0));
            Dir.Add(new Vector2Int(0, -1));

            foreach (var d in Dir)
            {
                ResCand = new PathNode(cand.Tile + d);
                if (!isWatched(ResCand) && isValidInGrid(ResCand.Tile) && isWallValid(cand.Tile, d)&&!isExcludePoint(ResCand.Tile))
                {
                    ResCand.Cost = cand.Cost + 1;
                    Result.Add(ResCand);
                }

            }

            return Result;

        }
        static bool isWatched(PathNode candidate)
        {
            return Watched.Where(t => t.Tile == candidate.Tile).FirstOrDefault() != null;
        }
        static bool isValidInGrid(Vector2Int tile)
        {
            int GSizeX = CurrentGrid.GetLength(0) -1;
            int GSizeY = CurrentGrid.GetLength(1) -1;

            return (tile.x >= 0 && tile.x < GSizeX) && (tile.y >= 0 && tile.y < GSizeY);
        }

        static bool isWallValid(Vector2Int from, Vector2Int Dir)
        {

            //1,0
            if (Dir.x == 1)
            {
                return !((MapTileObjectType)CurrentGrid[from.x + 1, from.y]).HasFlag(MapTileObjectType.VerticalWall);
            }

            //-1,0
            if (Dir.x == -1)
            {
                return !((MapTileObjectType)CurrentGrid[from.x, from.y]).HasFlag(MapTileObjectType.VerticalWall);
            }

            //0,1
            if (Dir.y == 1)
            {
                return !((MapTileObjectType)CurrentGrid[from.x, from.y + 1]).HasFlag(MapTileObjectType.HorizontalWall);
            }

            //0,-1
            if (Dir.y == -1)
            {
                return !((MapTileObjectType)CurrentGrid[from.x, from.y]).HasFlag(MapTileObjectType.HorizontalWall);
            }

            return false;
        }
        static bool isExcludePoint(Vector2Int cand)
        {
            return ExcludePoints.Any(t => t == cand);
        }
    }

    public class PathNode
    {
        public Vector2Int Tile;
        public int Cost;

        public PathNode(Vector2Int tile)
        {
            Tile = tile;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pathFinding;
[Flags]
public enum MapTileObjectType { Empty, VerticalWall = 1 << 1, HorizontalWall = 1 << 2, Coin = 1 << 3, BlueEnemy = 1 << 4, RedEnemy = 1 << 5, WhiteEnemy = 1 << 6, Player = 1 << 7 }

public class MapController : MonoBehaviour
{
    public static MapController Instance;

    [SerializeField]
    int TotalCoins = 15;


    [SerializeField]
    Transform PositionRootObject;

    [SerializeField]
    Transform WallsParent;

    [SerializeField]
    Transform CoinsParent;

    [SerializeField]
    Transform EntitiesParent;

    Transform PlayerPosition;



    List<GameObject> PathVisObj = new List<GameObject>();
    //пресеты поля лабиринта, пока набиты готовыми данными, но при дальнейшей разработке их можно 
    //полноценно генерировать каким-то алготимом
    //на 1 больше по размеру по каждой оси, чтобы закрыть стенами границы лабиринта
    int[,] TemplPreset =
    {
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0}

    };

    int[,] Preset1 =
     {
        {6,2,2,2,2,2,2,6,6,6,2,2,6,2,6,2,6,2,6,6,6,2,2,2,6,4},
        {6,0,4,2,2,2,0,0,0,0,2,4,0,4,0,4,0,4,0,0,0,2,4,2,0,4},
        {4,0,2,4,2,4,2,6,2,0,2,4,6,2,2,2,2,2,0,2,2,2,2,2,0,4},
        {6,2,4,6,4,4,4,4,4,4,2,4,4,4,2,4,4,4,2,2,2,2,2,6,4,4},
        {4,4,0,0,0,4,4,4,4,4,2,4,2,0,2,0,4,4,6,2,0,4,4,0,0,4},
        {4,2,2,2,0,4,4,4,4,4,2,4,4,4,6,0,4,4,0,2,2,0,6,2,4,4},
        {6,4,2,2,6,0,4,0,4,4,2,4,4,4,4,2,4,4,4,6,0,6,0,4,0,4},
        {4,2,0,6,0,6,2,2,0,0,2,0,2,0,4,2,0,4,4,4,6,0,2,2,2,4},
        {6,2,4,2,0,2,2,2,2,4,6,0,6,4,0,2,2,4,4,4,4,2,2,2,0,4},
        {4,4,2,2,4,6,2,2,0,4,4,4,0,4,2,4,2,4,4,4,4,2,2,0,0,4},
        {4,2,2,4,0,4,2,2,2,0,2,2,0,0,6,2,0,4,4,0,0,4,6,2,2,4},
        {4,4,0,2,4,6,2,2,2,0,6,2,70,4,6,2,0,0,2,4,2,4,2,2,0,4},
        {4,6,2,0,0,4,6,6,2,0,2,0,20,0,2,2,2,2,0,0,2,0,4,4,4,4},
        {4,0,0,6,0,4,0,4,2,2,2,0,36,4,6,2,0,6,2,6,2,4,4,4,4,4},
        {6,2,2,4,2,6,0,4,2,2,2,4,2,4,4,4,2,2,0,0,2,2,0,4,4,4},
        {4,6,4,2,4,4,2,2,2,2,4,4,2,0,4,4,4,6,2,2,6,6,0,4,4,4},
        {4,0,2,4,4,2,4,2,6,4,0,6,2,2,2,4,4,4,4,2,0,0,4,4,0,4},
        {4,6,0,0,2,4,4,4,0,4,4,2,4,4,0,0,4,4,4,4,6,4,4,0,2,4},
        {4,0,2,2,4,0,4,4,4,0,2,0,0,4,2,6,0,4,4,4,4,4,4,4,4,4},
        {4,2,4,4,2,2,4,2,4,6,0,2,6,4,4,4,2,0,4,0,0,0,4,4,4,4},
        {4,4,4,2,0,4,0,2,2,0,6,0,0,4,4,4,2,2,0,6,0,4,4,0,4,4},
        {4,4,2,2,0,2,4,2,2,4,2,2,0,0,4,4,4,2,4,4,2,4,2,2,0,4},
        {4,2,0,6,4,4,2,2,4,6,2,2,2,2,4,0,6,0,4,6,0,4,4,2,2,4},
        {4,6,2,0,2,0,4,2,4,0,0,4,0,4,4,4,4,2,4,4,2,0,6,2,4,4},
        {4,0,4,2,4,4,4,2,0,6,2,2,0,4,0,4,2,0,4,2,2,0,0,4,0,4},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0}
    };

    int[,] GenericPreset;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        GameResultVisual.HideResult();
        ClearLastGame();
        PlaceLabyrinth(Preset1);
        GameManager.CoinsTaken = 0;
        GameManager.CoinsLeft = TotalCoins;
        GameVisualInfo.UpdateCoinsInfo();
        GameManager.isGameStarted = true;
    }

    void ClearLastGame()
    {
        foreach(Transform i in WallsParent)
        {
            Destroy(i.gameObject);
        }
        foreach(Transform i in CoinsParent)
        {
            Destroy(i.gameObject);
        }
        foreach(Transform i in EntitiesParent)
        {
            Destroy(i.gameObject);
        }
    }

    void Update()
    {
        //часть логики, чтобы визуально собрать лабиринт.
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(GetFormatPreset(GenericPreset));
        }
    }


    void PlaceLabyrinthDetails(ref int[,] Grid)
    {
        int RandX;
        int RandY;
        int probe;

        int placeCoins = TotalCoins;


        //монетки
        while (placeCoins>0)
        {
            RandX = UnityEngine.Random.Range(0, 25);
            RandY = UnityEngine.Random.Range(0, 25);

            // 2 = флаг стены, 4 то же флаг стены, вместе = 6. и -6, т.е. если 0 и меньше, то клетка пуста для объекта
            probe = Grid[RandX, RandY] - 6;
            if (probe <= 0)
            {
                Grid[RandX, RandY] += (int)MapTileObjectType.Coin;
                placeCoins--;
            }
        }

        //игрок
        while (true)
        {
            RandX = UnityEngine.Random.Range(0, 25);
            RandY = UnityEngine.Random.Range(0, 25);

            // 2 = флаг стены, 4 то же флаг стены, вместе = 6. и -6, т.е. если 0 и меньше, то клетка пуста для объекта
            probe = Grid[RandX, RandY] - 6;
            if (probe <= 0 && !inRangeEnemy(ref Grid, 5, new Vector2Int(RandX, RandY)))
            {
                Grid[RandX, RandY] += (int)MapTileObjectType.Player;
                break;
            }
        }
    }

    bool inRangeEnemy(ref int[,] Grid, int radius, Vector2Int pos)
    {
        MapTileObjectType probe;
        for (int i = pos.x - radius; i < pos.x + radius; i++)
        {
            for (int b = pos.y - radius; b < pos.y + radius; b++)
            {
                if ((i >= 0 && Grid.GetLength(0) > i) && (b >= 0 && Grid.GetLength(1) > b))
                {
                    probe = (MapTileObjectType)Grid[i, b];
                    if (probe.HasFlag(MapTileObjectType.RedEnemy) || probe.HasFlag(MapTileObjectType.WhiteEnemy) || probe.HasFlag(MapTileObjectType.BlueEnemy))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void PlaceLabyrinth(int[,] Grid)
    {
        GenericPreset = Grid.Clone() as int[,];

        PlaceLabyrinthDetails(ref GenericPreset);

        GameObject WallInst = GameManager.Instance.WallInstance;
        GameObject CoinInst = GameManager.Instance.CoinInstance;
        GameObject PlayerInst = GameManager.Instance.PlayerInstance;
        GameObject WhiteEnemyInst = GameManager.Instance.WhiteEnemyInstance;
        GameObject BlueEnemyInst = GameManager.Instance.BlueEnemyInstance;
        GameObject RedEnemyInst = GameManager.Instance.RedEnemyInstance;
        GameObject newElem;

        //часть логики, чтобы визуально собрать лабиринт.
        WallInfo wi;

        for (int i = 0; i < 26; i++)
        {
            for (int b = 0; b < 26; b++)
            {
                MapTileObjectType probe = (MapTileObjectType)GenericPreset[i, b];
                if (probe.HasFlag(MapTileObjectType.VerticalWall))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i, 0, b);
                    newElem = Instantiate(WallInst, position, Quaternion.identity, WallsParent);

                    //часть логики, чтобы визуально собрать лабиринт.
                    wi = newElem.GetComponentInChildren<WallInfo>();
                    wi.pos = new Vector2Int(i, b);
                    wi.myType = MapTileObjectType.VerticalWall;

                }
                if (probe.HasFlag(MapTileObjectType.HorizontalWall))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i, 0, b);
                    newElem = Instantiate(WallInst, position, Quaternion.Euler(0f, 90f, 0f), WallsParent);

                    //часть логики, чтобы визуально собрать лабиринт.
                    wi = newElem.GetComponentInChildren<WallInfo>();
                    wi.pos = new Vector2Int(i, b);
                    wi.myType = MapTileObjectType.HorizontalWall;

                }
                if (probe.HasFlag(MapTileObjectType.Coin))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i + 0.5f, 0, b + 0.5f);
                    newElem = Instantiate(CoinInst, position, Quaternion.identity, CoinsParent);
                }

                if (probe.HasFlag(MapTileObjectType.Player))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i + 0.5f, 0.5f, b + 0.5f);
                    newElem = Instantiate(PlayerInst, position, Quaternion.identity, EntitiesParent);
                    PlayerPosition = newElem.transform;
                }

                if (probe.HasFlag(MapTileObjectType.WhiteEnemy))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i + 0.5f, 0.5f, b + 0.5f);
                    newElem = Instantiate(WhiteEnemyInst, position, Quaternion.identity, EntitiesParent);
                }

                if (probe.HasFlag(MapTileObjectType.BlueEnemy))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i + 0.5f, 0.5f, b + 0.5f);
                    newElem = Instantiate(BlueEnemyInst, position, Quaternion.identity, EntitiesParent);
                }

                if (probe.HasFlag(MapTileObjectType.RedEnemy))
                {
                    Vector3 position = PositionRootObject.position + new Vector3(i + 0.5f, 0.5f, b + 0.5f);
                    newElem = Instantiate(RedEnemyInst, position, Quaternion.identity, EntitiesParent);
                }
            }
        }
    }

    public static Vector2Int GetWoldRandomPoint()
    {
        return new Vector2Int(UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25)); ;
    }

    public static Vector3 GetWoldPlayerPosition()
    {
        return Instance.PlayerPosition.position;
    }

    public static Vector3 GetWoldPosFromGrid(Vector2Int pos)
    {
        return Instance.PositionRootObject.position + new Vector3(pos.x + 0.5f, 0.5f, pos.y + 0.5f);
    }

    public static Vector2Int GetGridPlayerPosition()
    {
        return GetInGridPos(Instance.PlayerPosition.position);
    }

    public static Vector2Int GetInGridPos(Vector3 v3pos)
    {
        Vector3 delta = v3pos - Instance.PositionRootObject.position;
        int X = Mathf.FloorToInt(delta.x);
        int Y = Mathf.FloorToInt(delta.z);
        return new Vector2Int(X, Y);
    }

    public static int[,] GetCurrentGrid()
    {
        return Instance.GenericPreset;
    }

    //визуализация поиска пути, для отладки
    public static void VizPath(List<PathNode> path)
    {
        ClearVizPath();
        GameObject ObjInst = GameManager.Instance.PathVisInstance;
        GameObject newObj;
        foreach (var i in path)
        {
            Vector3 position = Instance.PositionRootObject.position + new Vector3(i.Tile.x + 0.5f, 0, i.Tile.y + 0.5f);

            newObj = Instantiate(ObjInst, position, Quaternion.identity);

            newObj.GetComponentInChildren<Text>().text = i.Cost.ToString();
        }

    }

    static void ClearVizPath()
    {
        foreach (var i in Instance.PathVisObj)
        {
            Destroy(i);
        }
        Instance.PathVisObj.Clear();
    }
    #region visCreateLogic
    //часть логики, чтобы визуально собрать лабиринт.
    string GetFormatPreset(int[,] pres)
    {
        string result = "";

        string[] Dim1 = new string[26];
        string[] Dim2 = new string[26];

        for (int i = 0; i < 26; i++)
        {
            for (int b = 0; b < 26; b++)
            {
                Dim2[b] = pres[i, b].ToString();
            }
            Dim1[i] = "{" + string.Join(",", Dim2) + "}";
        }

        result = string.Join(",\n", Dim1);

        return result;
    }

    //часть логики, чтобы визуально собрать лабиринт.
    public void PostDestroyWall(Vector2Int pos, MapTileObjectType type)
    {
        GenericPreset[pos.x, pos.y] -= (int)type;
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isGameStarted;
    public static int CoinsLeft;
    public static int CoinsTaken;


    public GameObject WallInstance;
    public GameObject CoinInstance;
    public GameObject PlayerInstance;

    public GameObject WhiteEnemyInstance;
    public GameObject RedEnemyInstance;
    public GameObject BlueEnemyInstance;


    public GameObject PathVisInstance;

    void Awake()
    {
        Instance = this;
    }

    public static void TakeCoin()
    {
        CoinsLeft--;
        CoinsTaken++;
        GameVisualInfo.UpdateCoinsInfo();

        if(CoinsLeft == 0)
        {
            WinGame();
        }
    }
    static void WinGame()
    {
        isGameStarted = false;
        GameResultVisual.ShowWinResult();
    }
    public static void LoseGame()
    {
        isGameStarted = false;
        GameResultVisual.ShowLoseResult();
    }
}

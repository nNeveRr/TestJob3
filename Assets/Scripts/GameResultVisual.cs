using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultVisual : MonoBehaviour
{
    static GameResultVisual Instance;

    [SerializeField]
    GameObject MainPanel;

    [SerializeField]
    GameObject WinResPanel;

    [SerializeField]
    GameObject LoseResPanel;

    void Awake()
    {
        Instance = this;
    }

    public static void ShowWinResult()
    {
        Instance.MainPanel.SetActive(true);
        Instance.WinResPanel.SetActive(true);
        Instance.LoseResPanel.SetActive(false);
    }

    public static void ShowLoseResult()
    {
        Instance.MainPanel.SetActive(true);
        Instance.WinResPanel.SetActive(false);
        Instance.LoseResPanel.SetActive(true);
    }

    public static void HideResult()
    {
        Instance.MainPanel.SetActive(false);
    }
}

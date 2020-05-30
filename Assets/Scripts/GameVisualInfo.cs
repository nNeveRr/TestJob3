using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVisualInfo : MonoBehaviour
{
    public static GameVisualInfo Instance;

    [SerializeField]
    Text CoinsLeftText;

    [SerializeField]
    Text CoinsTakenText;

    public RectTransform VisualsTargetingBorder;

    void Awake()
    {
        Instance = this;
    }

    public static void UpdateCoinsInfo()
    {
        Instance.CoinsLeftText.text = GameManager.CoinsLeft.ToString();
        Instance.CoinsTakenText.text = GameManager.CoinsTaken.ToString();
    }
}

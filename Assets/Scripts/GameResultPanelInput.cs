using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultPanelInput : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            MapController.Instance.StartNewGame();
        }
    }
    public void RButton()
    {
        MapController.Instance.StartNewGame();
    }
}

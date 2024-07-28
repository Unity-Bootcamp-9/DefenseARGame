using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Subject : MonoBehaviour
{
    public event Action RedWin;
    public event Action BlueWin;

    public const int RedLayer = 6;
    public const int BlueLayer = 7;

    public bool gameEnd = false;
    public int winingTeam;
    public void Victory()
    {
        Debug.Log("승리");
        BlueWin?.Invoke();
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_ResultVictoryPopup>();
        winingTeam = 0;
    }

    public void Defeat()
    {
        Debug.Log("패배");
        RedWin?.Invoke();
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_ResultDefeatPopup>();
        winingTeam = 0;
    }

    public void SetResult(int _winingTeam)
    {
        gameEnd = true;
        winingTeam = _winingTeam;
    }

    private void Update()
    {
        if (gameEnd)
        {
            if (winingTeam == RedLayer)
                Victory();
            else if (winingTeam == BlueLayer)
                Defeat();
        }
        gameEnd = false;
    }


}

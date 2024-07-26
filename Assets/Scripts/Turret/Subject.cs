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

    public void Victory()
    {
        Debug.Log("승리");
        BlueWin?.Invoke();
    }

    public void Defeat()
    {
        Debug.Log("패배");
        RedWin?.Invoke();
    }

    private void Update()
    {
        if (gameEnd)
        {
            if (gameObject.layer == RedLayer)
                Victory();
            else if (gameObject.layer == BlueLayer)
                Defeat();
        }
        gameEnd = false;
    }


}

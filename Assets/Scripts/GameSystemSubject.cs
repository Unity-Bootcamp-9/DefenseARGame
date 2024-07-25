using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemSubject : MonoBehaviour
{
    public event Action RedWin;
    public event Action BlueWin;

    public void Victory()
    {
        BlueWin?.Invoke();
    }

    public void Defead()
    {
        RedWin?.Invoke();
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Popup
{
    placeMapPopup, inGamePopup
}

public class UIManager : MonoBehaviour
{
    private readonly Dictionary<Popup, Transform> _containers = new();

    private Image[] _manas;

    private void Awake()
    {
        int uiCount = Enum.GetValues(typeof(Popup)).Length;

        if (uiCount != transform.childCount)
        {
            Debug.LogError("UI 오브젝트 갯수 불일치. 자식 오브젝트를 넣거나 빼야 함.");
            return;
        }

        for (int i = 0; i < uiCount; i++)
        {
            _containers.Add((Popup)i, transform.GetChild(i));
        }

        _manas = _containers[Popup.inGamePopup].GetChild(0).GetChild(0)
            .GetComponentsInChildren<Image>();
    }

    public void ChangeTo(Popup value)
    {
        for (int i = 0; i < _containers.Count; i++)
        {
            GameObject self = _containers[(Popup)i].gameObject;
            self.SetActive((Popup)i == value);
        }
    }

    public void UpdateMana(int value)
    {
        for (int i = 1; i < _manas.Length; i++)
        {
            _manas[i].color = i <= value ? Color.cyan : Color.gray;
        }
    }
}
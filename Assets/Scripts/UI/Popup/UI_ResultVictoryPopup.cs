using System;
using System.Collections;
using UnityEngine;
using static Define;

public class UI_ResultVictoryPopup : UI_Popup
{
    enum Objects
    {
        VictoryImages,


    }
    enum Buttons
    {
        PlayAgainButton,
        ContinueButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PlayAgainButton).gameObject.BindEvent(OnClickPlayAgainButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        return true;
    }

    void OnClickPlayAgainButton()
    {
        Debug.Log("OnClickPlayAgainButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    void OnClickContinueButton()
    {
        Debug.Log("OnClickContinueButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();
    }

}

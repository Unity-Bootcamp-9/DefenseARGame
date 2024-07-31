using System;
using UnityEngine;
using static Define;

public class UI_ResultDefeatPopup : UI_Popup
{
    enum Buttons
    {
        ContinueButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);

        Managers.Sound.Clear();

        return true;
    }

    void OnClickContinueButton()
    {
        Debug.Log("OnClickContinueButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();

        Managers.Game.FinishGame();
    }

}

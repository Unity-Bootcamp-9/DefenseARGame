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
        ContinueButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);

        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Effect, "Stage Selected_Long_Rise_02");

        return true;
    }

    void OnClickContinueButton()
    {
        Debug.Log("OnClickContinueButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();

        Managers.Game.FinishGame();

        Managers.Sound.Play(Sound.Effect, "Confirm 1_UI_Impact_01");
    }

}

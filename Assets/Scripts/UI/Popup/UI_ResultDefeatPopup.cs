using System;
using UnityEngine;
using static Define;

public class UI_ResultDefeatPopup : UI_Popup
{
    enum Buttons
    {
        PlayAgainButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PlayAgainButton).gameObject.BindEvent(OnClickPlayAgainButton);

        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Effect, "Defeated_Fail_Effect_02");

        return true;
    }

    void OnClickPlayAgainButton()
    {
        Debug.Log("OnClickPlayAgainButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();

        Managers.Game.FinishGame();

        Managers.Sound.Play(Sound.Effect, "Confirm 1_UI_Impact_01");
    }

}

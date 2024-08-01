using System;
using UnityEngine;
using static Define;

public class UI_TitlePopup : UI_Popup
{
    enum Buttons
    {
        BackGroundButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackGroundButton).gameObject.BindEvent(OnClickBackGroundButton);

        return true;
    }

    void OnClickBackGroundButton()
    {
        Debug.Log("OnClickBackGroundButton");

        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();

        Managers.Sound.Play(Sound.Effect, "Confirm 1_UI_Impact_01");
    }

}

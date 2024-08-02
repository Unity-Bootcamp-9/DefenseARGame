using System;
using UnityEngine;
using static Define;

public class UI_MapSettingPopup : UI_Popup
{
    enum Buttons
    {
        MapSettingButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.MapSettingButton).gameObject.BindEvent(OnClickMapSettingButton);

        return true;
    }

    void OnClickMapSettingButton()
    {
        Debug.Log("OnClickMapSettingButton");
        if (Managers.Game.StartGame())
        {
            Managers.Resource.Load<Material>("Materials/M_Plane").color = Color.clear;
            Managers.UI.ClosePopupUI(this);
            Managers.UI.ShowPopupUI<UI_BattlePopup>();
            Managers.Sound.Play(Sound.Effect, "Confirm 1_UI_Impact_01");
        }
    }
}

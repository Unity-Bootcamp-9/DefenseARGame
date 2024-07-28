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

        return true;
    }

    void OnClickContinueButton()
    {
        Debug.Log("OnClickContinueButton");
        Managers.UI.ClosePopupUI(this);

        Managers.UI.ShowPopupUI<UI_LevelPopup>();

        Managers.Resource.Load<Material>("Materials/M_Plane").color = new Color(1f, 1f, 0f, 0.05f);
    }

}

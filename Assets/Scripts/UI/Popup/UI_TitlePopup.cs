using System;
using UnityEngine;
using static Define;

public class UI_TitlePopup : UI_Popup
{
/*    enum Texts
    {
        TouchToStartText,
    }
*/
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

        Managers.UI.ClosePopupUI(this); // UI_TitlePopup

        Managers.UI.ShowPopupUI<UI_LevelPopup>();
    }

}

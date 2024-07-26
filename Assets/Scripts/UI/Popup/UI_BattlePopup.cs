using System;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
/*    enum Buttons
    {
        MapSettingButton,
    }
*/
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

/*        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.MapSettingButton).gameObject.BindEvent(OnClickMapSettingButton);

*/

        return true;
    }

/*    void OnClickMapSettingButton()
    {
        Debug.Log("OnClickMapSettingButton");
        Managers.Game.StartGame();
        Managers.UI.ClosePopupUI(this);

        //Managers.UI.ShowPopupUI<UI_BattlePopup>();

    }
*/
}

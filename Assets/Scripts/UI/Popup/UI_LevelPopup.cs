using System;

public class UI_LevelPopup : UI_Popup
{
    enum Buttons
    {
        LevelSelectButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.LevelSelectButton).gameObject.BindEvent(OnClickLevelSelectButton);
        return true;
    }

    void OnClickLevelSelectButton()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();

        Managers.Game.ReadyGame();

        Managers.Sound.Play(Define.Sound.Effect, "Confirm 1_UI_Impact_01");
    }

}

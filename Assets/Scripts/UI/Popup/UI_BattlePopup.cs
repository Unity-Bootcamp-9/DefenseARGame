using System;
using TMPro;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    enum Texts
    {
        PlayTimeText,
    }

    enum Images
    {
        Skill1Image,
        Skill2Image,
        Skill3Image,
        Skill4Image,
        Steminas,
    }

    enum Buttons
    {
        PauseButton,
    }

    float sec;
    int min;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        min = 0;
        sec = 0;

        GetText((int)Texts.PlayTimeText).text = "00:00";

        GetImage((int)Images.Skill1Image).gameObject.GetOrAddComponent<MeteorSkill>();
        GetImage((int)Images.Skill2Image).gameObject.GetOrAddComponent<MonsoonSkill>();
        GetImage((int)Images.Skill3Image).gameObject.GetOrAddComponent<Skill>();
        GetImage((int)Images.Skill4Image).gameObject.GetOrAddComponent<HealSkill>();
        GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>().FindListener();

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

        return true;
    }

    private void Update()
    {
        sec += Time.deltaTime;
        GetText((int)Texts.PlayTimeText).text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if ((int)sec > 59)
        {
            sec = 0;
            min++;
        }

    }

    private void OnClickPauseButton()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
        Managers.Game.PauseGame();
    }
}

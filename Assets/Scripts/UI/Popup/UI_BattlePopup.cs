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
    Mana _mana;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        min = Managers.Game.PlayData.minute;
        sec = Managers.Game.PlayData.second;

        GetText((int)Texts.PlayTimeText).text = "00:00";

        GetImage((int)Images.Skill1Image).gameObject.GetOrAddComponent<MeteorSkill>();
        GetImage((int)Images.Skill2Image).gameObject.GetOrAddComponent<MonsoonSkill>();
        GetImage((int)Images.Skill3Image).gameObject.GetOrAddComponent<Skill>();
        GetImage((int)Images.Skill4Image).gameObject.GetOrAddComponent<HealSkill>();

        _mana = GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>();
        _mana.FindListener();
        _mana.UpdateMana(-10 + Managers.Game.PlayData.currentMana);

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        
        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Bgm, "BGM");

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
        Managers.Game.PlayData = new PlayData(_mana.CurrentMana, min, sec);
    }
}

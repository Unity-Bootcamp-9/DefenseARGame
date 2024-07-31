using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    // enum을 정의할 때, 0번째부터 3번째 까지는 반드시 Tooltip 관련 오브젝트 이름을 순서대로 정의해주세요.
    enum GameObjects
    {
        ToolTip1,
        ToolTip2,
        ToolTip3,
        ToolTip4,
    }

    enum Texts
    {
        ToolTipText1,
        ToolTipText2,
        ToolTipText3,
        ToolTipText4,
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
        Skill1Image,
        Skill2Image,
        Skill3Image,
        Skill4Image,
        PauseButton,
    }

    float sec;
    int min;
    bool isOnClick;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        RefreshUI();

        return true;
    }

    private void RefreshUI()
    {
        GetImage((int)Images.Skill1Image).gameObject.GetOrAddComponent<MeteorSkill>();
        GetImage((int)Images.Skill2Image).gameObject.GetOrAddComponent<MonsoonSkill>();
        GetImage((int)Images.Skill3Image).gameObject.GetOrAddComponent<MonsoonSkill>(); // 임시
        GetImage((int)Images.Skill4Image).gameObject.GetOrAddComponent<HealSkill>();

        GetText((int)Texts.PlayTimeText).text = "00:00";

        GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>().FindListener();

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

        SetTooltipInfo();

        min = 0;
        sec = 0;
        isOnClick = false;
    }

    private void SetTooltipInfo()
    {
        Skill skill;
        for(int i  = 0; i < 4; i++)
        {
            skill = GetImage(i).gameObject.GetOrAddComponent<Skill>();
            GetText(i).text = $"스킬 이름 : {skill.SkillName_KR}\n스킬 설명 : {skill.Description}\n필요 마나 : {skill.RequireMana}\n피해량 : {skill.Damage}\n스킬 범위 : {skill.Radius}";
            GetButton(i).gameObject.BindEvent(OnPressImage, Define.UIEvent.Pressed);
            GetButton(i).gameObject.BindEvent(OnPointerUpImage, Define.UIEvent.PointerUp);
            GetObject(i).SetActive(false);
        }
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

    float time = 0;

    int index = -1;

    void OnPressImage()
    {
        time = 0;
        if (time > 1f)
        {
            GetObject(index).SetActive(true);
        }
        else
        {
            time += Time.deltaTime;
        }
    }
    void OnPointerUpImage()
    {
        gameObject.SetActive(false);
    }

    // 손 떼면 온클릭 false로 바꾸고 툴팁 오프
}

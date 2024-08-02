using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    enum GameObjects
    {
        ToolTip1,
        ToolTip2,
        ToolTip3,
        ToolTip4,
        AfterPause,
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
        PauseButton,
        MapSettingButton,
        BackToMainButton,
        ContinueButton,
    }

    float sec;
    int min;

    private float[] _pressTimes;
    private bool[] _isPressing;

    Mana _mana;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        _pressTimes = new float[Enum.GetValues(typeof(Images)).Length];
        _isPressing = new bool[Enum.GetValues(typeof(Images)).Length];

        min = Managers.Game.PlayData.minute;
        sec = Managers.Game.PlayData.second;

        RefreshUI();

        return true;
    }

    private void RefreshUI()
    {
        GetText((int)Texts.PlayTimeText).text = "00:00";

        GetImage((int)Images.Skill1Image).gameObject.GetOrAddComponent<MeteorSkill>();
        GetImage((int)Images.Skill2Image).gameObject.GetOrAddComponent<MonsoonSkill>();
        GetImage((int)Images.Skill3Image).gameObject.GetOrAddComponent<IceSkill>(); // 임시
        GetImage((int)Images.Skill4Image).gameObject.GetOrAddComponent<HealSkill>();

        GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>().FindListener();
        
        _mana = GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>();
        _mana.FindListener();
        _mana.UpdateMana(-10 + Managers.Game.PlayData.currentMana);

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.MapSettingButton).gameObject.BindEvent(OnClickMapSettingButton);
        GetButton((int)Buttons.BackToMainButton).gameObject.BindEvent(OnClickBackToMainButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);

        SetTooltipInfo();

        GetObject((int)GameObjects.AfterPause).SetActive(false);

        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Bgm, "BGM");

        min = 0;
        sec = 0;
    }

    private void SetTooltipInfo()
    {
        Skill skill;
        for (int i = 0; i < 4; i++)
        {
            int index = i; // 클로저 문제 해결을 위해 지역 변수 사용
            skill = GetImage(i).gameObject.GetOrAddComponent<Skill>();
            if (skill.Damage > 0)
            {
                GetText(i).text = $"스킬 이름 : {skill.SkillName_KR}\n스킬 설명 : {skill.Description}\n필요 마나 : {skill.RequireMana}\n피해량 : {skill.Damage}\n스킬 범위 : {skill.Radius}";
            }
            else if(skill.Damage < 0)
            {
                GetText(i).text = $"스킬 이름 : {skill.SkillName_KR}\n스킬 설명 : {skill.Description}\n필요 마나 : {skill.RequireMana}\n회복량 : {-skill.Damage}\n스킬 범위 : {skill.Radius}";
            }
            else
            {
                GetText(i).text = $"스킬 이름 : {skill.SkillName_KR}\n스킬 설명 : {skill.Description}\n필요 마나 : {skill.RequireMana}\n스킬 범위 : {skill.Radius}";
            }
            GetImage(i).gameObject.BindEvent(() => OnPointerDownImage(index), Define.UIEvent.PointerDown);
            GetImage(i).gameObject.BindEvent(() => OnPointerUpImage(index), Define.UIEvent.PointerUp);
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

        // 누르고 있는 시간 체크
        for (int i = 0; i < 4; i++)
        {
            if (_isPressing[i])
            {
                _pressTimes[i] += Time.deltaTime;
                if (_pressTimes[i] >= 1.0f)
                {
                    GetObject(i).gameObject.SetActive(true);
                    _isPressing[i] = false;
                }
            }
        }
    }

    private void OnClickPauseButton()
    {
        Time.timeScale = 0f;
        GetObject((int)GameObjects.AfterPause).SetActive(true);
    }

    void OnPointerDownImage(int index)
    {
        Debug.Log("OnPointerDownImage");
        _isPressing[index] = true;
        _pressTimes[index] = 0;
    }

    void OnPointerUpImage(int index)
    {
        Debug.Log("OnPointerUpImage");
        GetObject(index).SetActive(false);
        _isPressing[index] = false;
    }

    private void OnClickMapSettingButton()
    {
        Time.timeScale = 1.0f;
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();

        Managers.Game.PauseGame();
        Managers.Game.PlayData = new PlayData(_mana.CurrentMana, min, sec);
    }

    private void OnClickContinueButton()
    {
        Time.timeScale = 1.0f;
        GetObject((int)GameObjects.AfterPause).SetActive(false);
    }

    private void OnClickBackToMainButton()
    {
        Time.timeScale = 1.0f;
        Managers.Game.FinishGame();
        Managers.Resource.Load<Material>("Materials/M_Plane").color = new Color(1f, 1f, 0f, 0.05f);

        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_LevelPopup>();
    }

}

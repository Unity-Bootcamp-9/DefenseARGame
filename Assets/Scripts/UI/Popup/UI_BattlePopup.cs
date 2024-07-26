using System;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    enum Texts
    {
        PlayTimeText
    }

    enum Buttons
    {
        Skill1Button,
        Skill2Button,
        Skill3Button,
        Skill4Button,
    }

    enum States
    {
        Defeat,
        Victory,
        Default
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Buttons));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Skill1Button).gameObject.BindEvent(OnClickSkill1Button);
        GetButton((int)Buttons.Skill2Button).gameObject.BindEvent(OnClickSkill2Button);
        GetButton((int)Buttons.Skill3Button).gameObject.BindEvent(OnClickSkill3Button);
        GetButton((int)Buttons.Skill4Button).gameObject.BindEvent(OnClickSkill4Button);

        return true;
    }

    void OnClickSkill1Button()
    {
        Debug.Log("OnClickSkill1Button");
    }

    void OnClickSkill2Button()
    {
        Debug.Log("OnClickSkill2Button");
    }

    void OnClickSkill3Button()
    {
        Debug.Log("OnClickSkill3Button");
    }

    void OnClickSkill4Button()
    {
        Debug.Log("OnClickSkill4Button");
    }

    States state = States.Default;
    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            state = States.Victory;
        }
        switch (state)
        {
            case States.Defeat:
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_ResultDefeatPopup>();
                break;
            case States.Victory:
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_ResultVictoryPopup>();
                break;
            default:
                break;
        }
    }

}

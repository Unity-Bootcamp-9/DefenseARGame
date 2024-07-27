using System;
using UnityEngine;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    enum Texts
    {
        PlayTimeText
    }

    enum Images
    {
        Skill1Image,
        Skill2Image,
        Skill3Image,
        Skill4Image,
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
        BindImage(typeof(Images));

        GetImage((int)Images.Skill1Image).gameObject.BindEvent(OnClickSkill1Image);
        GetImage((int)Images.Skill2Image).gameObject.BindEvent(OnClickSkill2Image);
        GetImage((int)Images.Skill3Image).gameObject.BindEvent(OnClickSkill3Image);
        GetImage((int)Images.Skill4Image).gameObject.BindEvent(OnClickSkill4Image);

        return true;
    }

    void OnClickSkill1Image()
    {
        Debug.Log("OnClickSkill1Image");
    }

    void OnClickSkill2Image()
    {
        Debug.Log("OnClickSkill2Image");
    }

    void OnClickSkill3Image()
    {
        Debug.Log("OnClickSkill3Image");
    }

    void OnClickSkill4Image()
    {
        Debug.Log("OnClickSkill4Image");
    }

    States state = States.Default;
    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        
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

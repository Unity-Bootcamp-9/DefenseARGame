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

    enum States
    {
        Defeat,
        Victory,
        Default
    }

    States state = States.Default;
    float sec;  //Time.deltatime 이 float형태로 반환하기 때문에 float을 써준다.
    int min;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        min = 0;
        sec = 0;

        GetText((int)Texts.PlayTimeText).text = "00:00";

        GetImage((int)Images.Skill1Image).gameObject.GetOrAddComponent<RapidFireSkill>();
        GetImage((int)Images.Skill2Image).gameObject.GetOrAddComponent<RapidFireSkill>();
        GetImage((int)Images.Skill3Image).gameObject.GetOrAddComponent<RapidFireSkill>();
        GetImage((int)Images.Skill4Image).gameObject.GetOrAddComponent<RapidFireSkill>();
        GetImage((int)Images.Steminas).gameObject.GetOrAddComponent<Mana>().FindListener();

        return true;
    }

    private void Update()
    {
        sec += Time.deltaTime;
        GetText((int)Texts.PlayTimeText).text = string.Format("{0:D2}:{1:D2}", min, (int)sec); //bool에서 float이엿기때문에 (int)넣어줌

        if ((int)sec > 59) //1분은 60초 이기 때문에 초는 59초 까지로 범위를 설정
        {
            sec = 0; //sec의 기본값은 0
            min++;  //sec가 59보다 커질때 1분이 될때 Min(분) 은 커진다.
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

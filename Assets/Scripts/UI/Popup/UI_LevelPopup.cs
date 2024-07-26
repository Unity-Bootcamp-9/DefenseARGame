using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using static Define;

public class UI_LevelPopup : UI_Popup
{
    enum Texts
    {
        LevelSelectText,
    }

    enum Buttons
    {
        LevelSelectButton,
        BlankSpaceButton,
        HardButton,
        MiddleButton,
        LowButton,
    }

    public float MovingSpeed = 100.0f;
    public Vector3 upPos = new Vector3(800, 400, 0);
    public Vector3 downPos = new Vector3(800, -7000, 0);
    public RectTransform selectSection;

    bool up;
    bool down;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.LevelSelectButton).gameObject.BindEvent(OnClickLevelSelectButton);
        GetButton((int)Buttons.BlankSpaceButton).gameObject.BindEvent(OnClickBlankSpaceButton);
        GetButton((int)Buttons.HardButton).gameObject.BindEvent(OnClickHardButton);
        GetButton((int)Buttons.MiddleButton).gameObject.BindEvent(OnClickMiddleButton);
        GetButton((int)Buttons.LowButton).gameObject.BindEvent(OnClickLowButton);

        return true;
    }

    void OnClickLevelSelectButton()
    {
        Debug.Log("OnClickLevelSelectButton");
        down = false;
        up = true;
    }

    void OnClickBlankSpaceButton()
    {
        Debug.Log("OnClickBlankSpaceButton");
        down = true;
        up = false;
    }

    void OnClickHardButton()
    {
        Debug.Log("OnClickHardButton");
        // 아직 해금 안됨
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    void OnClickMiddleButton()
    {
        Debug.Log("OnClickMiddleButton");
        // 아직 해금 안됨
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();

    }

    void OnClickLowButton()
    {
        Debug.Log("OnClickLowButton");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    void GoUp() => selectSection.position = Vector3.MoveTowards(selectSection.position, upPos, MovingSpeed);
    void GoDown() => selectSection.position = Vector3.MoveTowards(selectSection.position, downPos, MovingSpeed);

    void Update()
    {
        if (up) GoUp();
        else if(down) GoDown();
    }


}

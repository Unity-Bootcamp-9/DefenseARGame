using System;
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

    enum SelectState
    {
        Default,
        BeforeSelect,
        AfterSelect
    }

    SelectState selectState = SelectState.Default;

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
        selectState = SelectState.AfterSelect;
    }

    void OnClickBlankSpaceButton()
    {
        Debug.Log("OnClickBlankSpaceButton");
        selectState = SelectState.BeforeSelect;
    }

    void OnClickHardButton()
    {
        Debug.Log("OnClickHardButton");
        // 아직 해금 안됨
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    void OnClickMiddleButton()
    {
        Debug.Log("OnClickMiddleButton");
        // 아직 해금 안됨
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    void OnClickLowButton()
    {
        Debug.Log("OnClickLowButton");
        Managers.UI.ShowPopupUI<UI_MapSettingPopup>();
    }

    public float MovingSpeed = 500.0f;
    public Vector3 upPos = new Vector3(0, 0, 0);
    public Vector3 downPos = new Vector3(0, -1000, 0);
    public RectTransform selectSection;

    void GoUp()
    {
        if (selectSection.position.y > upPos.y)
        {
            SelectState selectState = SelectState.Default;
            return;
        }
        else
        {
            selectSection.position += MovingSpeed * Vector3.up * Time.deltaTime;
        }
    }

    void GoDown()
    {
        if (selectSection.position.y < downPos.y)
        {
            SelectState selectState = SelectState.Default;
            return;
        }
        else
        {
            selectSection.position += MovingSpeed * Vector3.down * Time.deltaTime;
        }
    }

    void Update()
    {
        switch (selectState)
        {
            case SelectState.BeforeSelect:
                GoDown();
                break;
            case SelectState.AfterSelect:
                GoUp();
                break;
            case SelectState.Default:
                break;
            default:
                break;
        }
    }


}

using System;
using UnityEngine.SceneManagement;

public class UI_ConfirmDialog : UI_Popup
{
    enum Texts
    {
        TitleText,
        BodyText,
        ConfirmText
    }

    enum Buttons
    {
        Confirm
    }

    string _title;
    string _body;
    string _confirm;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Confirm).gameObject.BindEvent(OnClickYesButton);

        GetText((int)Texts.TitleText).text = _title;
        GetText((int)Texts.BodyText).text = _body;
        GetText((int)Texts.ConfirmText).text = _confirm;

        return true;
    }

    public void SetDialog(Action onClickYesButton, string title, string body, string confirm)
    {
        _onClickYesButton = onClickYesButton;
        _title = title;
        _body = body;
        _confirm = confirm;
    }

    Action _onClickYesButton;
    void OnClickYesButton()
    {
        Managers.UI.ClosePopupUI(this);
        if (_onClickYesButton != null)
            _onClickYesButton.Invoke();
    }

    void OnComplete()
    {
        Managers.UI.ClosePopupUI(this);
    }
}

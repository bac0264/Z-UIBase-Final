using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zitga.Localization;

public enum TextCaseComponent
{
    None,
    UpperCase,
    LowerCase,
    CamelCase,
}

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class LocalizeHelper : MonoBehaviour
{
    public TextCaseComponent caseSelect = TextCaseComponent.None;
    public string Category;
    public string Key;
    private Text _textComponent;

    public string Value
    {
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _textComponent.text = $"#{Category}_{Key}";
            }
            else
            {
                FinalizeText(value);
            }
        }
    }

    private bool _mStarted = false;

    void OnEnable ()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        if (_mStarted)
            OnLocalize();
    }

    void Start ()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        _textComponent = GetComponent<Text>();
        _mStarted = true;
        OnLocalize();
//            this.RegisterListener(EventID.ChangeLanguage, (sender, param) =>
//             {
//                 Value = Localize.GetLocalizedString(Key);
//             });
    }

    void OnLocalize ()
    {
        // If no localization key has been specified, use the label's text as the key
        if (string.IsNullOrEmpty(Key))
        {
            if (_textComponent != null) Key = _textComponent.text;
        }

        // If we still don't have a key, leave the value as default text in Text Component
        if (!string.IsNullOrEmpty(Key))
        {
            Value = Localization.Current.Get(Category, Key);
        }
    }

    private void FinalizeText(string text)
    {
        switch (caseSelect)
        {
            case TextCaseComponent.None:
                _textComponent.text = text;
                break;
            case TextCaseComponent.UpperCase:
                _textComponent.text = text.ToUpper();
                break;
            case TextCaseComponent.LowerCase:
                _textComponent.text = text.ToLower();
                break;
            // case TextCaseComponent.CamelCase:
            //     _textComponent.text = UtilityGame.ToCamelCase(text);
            //     break;
        }
    }

}

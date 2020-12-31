using System;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SelectWorldProperties : WindowProperties
{
    public readonly int NumberScroll;

    public SelectWorldProperties(int numberScroll)
    {
        NumberScroll = numberScroll;
    }
}

public class SelectWorldWindowController : AWindowController <SelectWorldProperties>
{
    [SerializeField] private Text number;
    
    public void CloseWindow()
    {
        UIFrame.Instance.CloseCurrentWindow();
    }
    
    protected override void OnPropertiesSet()
    {
        number.text = Properties.NumberScroll.ToString();
    }
}

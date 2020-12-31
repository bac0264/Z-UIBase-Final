using deVoid.UIFramework;

public class ShopPopupController : AWindowController
{
    public void ClosePopup()
    {
        UIFrame.Instance.CloseCurrentWindow();        
    }
}

using deVoid.UIFramework;
using deVoid.UIFramework.Examples.WordChef;
using Random = UnityEngine.Random;

public class HomeWindowController : AWindowController
{
    public void OpenShop()
    {
        UIFrame.Instance.OpenWindow(ScreenIds.ShopWindow);
    }

    public void SelectWorld()
    {
        UIFrame.Instance.OpenWindow(ScreenIds.SelectWorldWindow, new SelectWorldProperties(Random.Range(0, 20)));
    }
}

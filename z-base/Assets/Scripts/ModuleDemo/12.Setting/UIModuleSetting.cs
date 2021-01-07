using deVoid.UIFramework;

public class UIModuleSetting : AWindowController
{
    public SoundAndMusicView soundAndMusicView;
    public LanguageView languageView;

    protected override void OnPropertiesSet()
    {
        soundAndMusicView.InitOrUpdateView();
        languageView.InitOrUpdateView();
    }
}

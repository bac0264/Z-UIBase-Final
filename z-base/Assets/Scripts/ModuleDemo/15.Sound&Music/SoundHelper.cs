using UnityEngine;
using UnityEngine.UI;

public class SoundHelper : MonoBehaviour
{
    public SoundUI sound = SoundUI.NONE;

    public Button buttonHelper;

    private void OnValidate()
    {
        if (buttonHelper == null) buttonHelper = GetComponent<Button>();
    }

    private void Awake()
    {
        buttonHelper.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        SoundManager.instance.OnPlaySoundUI(sound);
    }
}

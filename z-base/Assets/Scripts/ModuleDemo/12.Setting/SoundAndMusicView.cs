using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAndMusicView : MonoBehaviour
{
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button musicBtn;

    [SerializeField] private Text soundText;
    [SerializeField] private Text musicText;
    
    private PlayerSetting playerSetting;
    
    public void InitOrUpdateView()
    {
        playerSetting = DataPlayer.GetModule<PlayerSetting>();
        
        soundBtn.onClick.AddListener(OnClickSound);
        musicBtn.onClick.AddListener(OnClickMusic);

        UpdateView();
    }

    private void UpdateView()
    {
        UpdateSoundView();
        UpdateMusicView();
    }

    private void UpdateSoundView()
    {
        soundText.text = playerSetting.IsOpenSound() ? "On" : "Off";
    }

    private void UpdateMusicView()
    {
        musicText.text = playerSetting.IsOpenMusic() ? "On" : "Off";
    }
    
    private void OnClickMusic()
    {
        playerSetting.SetOpenMusic();
        UpdateMusicView();
    }

    private void OnClickSound()
    {
        playerSetting.SetOpenSound();
        UpdateSoundView();
    }
}

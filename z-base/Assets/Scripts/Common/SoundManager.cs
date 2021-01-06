using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum SoundUI
{
    NONE = -1,
    SFX_UI_BACK = 0,
    SFX_UI_SELECT = 1,
    SFX_UI_POPUP_CLOSE = 2,
    SFX_UI_POPUP_OPEN = 3,
}

public enum SoundGamePlay
{
    NONE = -1,
}

public class SoundManager : MonoBehaviour
{

    public AudioSource audioFX;
    
    public static SoundManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SetAudioActive();
    }

    private void OnValidate()
    {
        if (audioFX == null)
        {
            audioFX = GetComponent<AudioSource>();
            if (audioFX == null) audioFX = gameObject.AddComponent<AudioSource>();
            audioFX.playOnAwake = false;
        }
    }

    public void SetAudioActive()
    {
        audioFX.mute = !DataPlayer.GetModule<PlayerSetting>().IsOpenSound();
    }
    
    public void StopAllSound()
    {

        audioFX.Stop();
    }

    public void MuteAllSound()
    {

        audioFX.mute = true;
    }

    public void UnMuteAllSound()
    {

        audioFX.mute = false;
    }

    public void OnPlaySoundUI(SoundUI playSoundType)
    {
        if (playSoundType == SoundUI.NONE) return;
        
        var audio = LoadResourceController.GetSoundUI(playSoundType);
        audioFX.PlayOneShot(audio);
    }
    
    public void OnPlaySoundGamePlay(SoundGamePlay playSoundType)
    {
        if (playSoundType == SoundGamePlay.NONE) return;
        
        var audio = LoadResourceController.GetSoundGamePlay(playSoundType);
        audioFX.PlayOneShot(audio);
    }
}
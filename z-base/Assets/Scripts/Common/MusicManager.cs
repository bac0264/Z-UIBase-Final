using System.Collections;
using UnityEngine;

public enum MusicType
{
    M_BATTLE_PVP = 1,
}
public class MusicManager : MonoBehaviour
{
    public AudioSource audioBG;

    [Header("---------------------BG Music Menu----------------------")]

    public bool muteBGMusic;
    public static MusicManager instance;

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
        OnPlayMusic(MusicType.M_BATTLE_PVP);
    }

    public void OnPlayMusic(MusicType musicType)
    {
        audioBG.Stop();
        if (!audioBG.isPlaying && muteBGMusic == false)
        {
            FadeSoundOn(audioBG, 1, 0f, 1f);
            audioBG.clip = LoadResourceController.GetMusic(musicType);
            audioBG.PlayDelayed(1f);
            audioBG.loop = true;
            audioBG.Play();
        }
    }

    private void OnValidate()
    {
        if (audioBG == null)
        {
            audioBG = GetComponent<AudioSource>();
            if (audioBG == null) audioBG = gameObject.AddComponent<AudioSource>();
            audioBG.playOnAwake = false;
        }
    }
    
    public void SetAudioActive()
    {
        audioBG.mute = !DataPlayer.GetModule<PlayerSetting>().IsOpenMusic();
    }
    //------------------------------------------------------------------------------------------------
    public void StopBGMusic()
    {
        audioBG.Stop();
    }

    public void PauseBGMusic()
    {
        audioBG.Pause();
    }

    public void UnPauseBGMusic()
    {
        audioBG.UnPause();
    }

    public void FadeSoundOn(AudioSource audioSrc, float fadeTime, float minVolume, float maxVolume)
    {
        StartCoroutine(_FadeSoundOn(audioSrc, fadeTime, minVolume, maxVolume));
    }

    IEnumerator _FadeSoundOn(AudioSource audioSrc, float fadeTime, float minVolume, float maxVolume)
    {
        float t = minVolume;
        while (t < fadeTime * maxVolume)
        {
            yield return null;
            t += Time.deltaTime;
            audioSrc.volume = t / fadeTime;
        }

        yield break;
    }

    public void FadeSoundOff(AudioSource audioSrc, float fadeTime, float minVolume, float maxVolume)
    {
        StartCoroutine(_FadeSoundOff(audioSrc, fadeTime, minVolume, maxVolume));
    }

    IEnumerator _FadeSoundOff(AudioSource audioSrc, float fadeTime, float minVolume, float maxVolume)
    {
        float t = fadeTime * maxVolume;
        while (t > minVolume)
        {
            yield return null;
            t -= Time.deltaTime;
            audioSrc.volume = t / fadeTime;
            if (audioSrc.volume < 0.01f)
            {
                audioSrc.Stop();
            }
        }

        yield break;
    }
}
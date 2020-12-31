using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IronSourceManager : MonoBehaviour
{
    public static string INTERSTITIAL_INSTANCE_ID = "0";
    public static string REWARDED_INSTANCE_ID = "0";
    public static string uniqueUserId = "demoUserUnity";
#if UNITY_ANDROID
    public static string appKey = "a9bcf2dd";
#elif UNITY_IOS
    public static string appKey = "bc8cb805";
#else
    public static string appKey = "a9bcf2dd";
#endif
    private Action callback;
    public static IronSourceManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Debug.Log("unity-script: MyAppStart Start called");

        //IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        // Add Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;


        // Add Interstitial DemandOnly Events
        IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;

        // Add Rewarded Interstitial Events
        // IronSourceEvents.onInterstitialAdRewardedEvent += InterstitialAdRewardedEvent;

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);
        //IronSource.Agent.init (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
        //IronSource.Agent.initISDemandOnly (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);

        //Set User ID For Server To Server Integration
        //// IronSource.Agent.setUserId ("UserId");

        // Load Banner example
        LoadBanner();
        LoadInterstitial();
    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void LoadBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }

    public void ShowBanner()
    {
        LoadBanner();
    }

    //Banner Events
    public void BannerAdLoadedEvent()
    {
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " +
                  error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }

    /************* Interstitial API *************/
    public bool isInterstitialReady()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            return IronSource.Agent.isInterstitialReady();
        else
            return false;
    }

    public void LoadInterstitial()
    {
        //Debug.Log("unity-script: LoadInterstitial");
        IronSource.Agent.loadInterstitial();

        //DemandOnly
        // LoadDemandOnlyInterstitial ();
    }

    public void ShowInterstitial()
    {
        //Debug.Log("unity-script: ShowInterstitialButtonClicked");
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;
        if (isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            //Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
            LoadInterstitial();
        }

        // DemandOnly
        // ShowDemandOnlyInterstitial ();
    }

    void LoadDemandOnlyInterstitial()
    {
        Debug.Log("unity-script: LoadDemandOnlyInterstitialButtonClicked");
        IronSource.Agent.loadISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
    }

    void ShowDemandOnlyInterstitial()
    {
        Debug.Log("unity-script: ShowDemandOnlyInterstitialButtonClicked");
        if (IronSource.Agent.isISDemandOnlyInterstitialReady(INTERSTITIAL_INSTANCE_ID))
        {
            IronSource.Agent.showISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyInterstitialReady - False");
        }
    }

    #region /************* Interstitial Delegates *************/

    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " +
                  error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " +
                  error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        LoadInterstitial();
    }

    void InterstitialAdRewardedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdRewardedEvent");
    }

    /************* Interstitial DemandOnly Delegates *************/

    void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId +
                  ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId +
                  ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdRewardedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdRewardedDemandOnlyEvent for instance: " + instanceId);
    }

    #endregion

    /************* RewardedVideo API *************/
    public bool IsVideoRewardReady()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            return IronSource.Agent.isRewardedVideoAvailable();
        else
            return false;
    }

    public void ShowRewardedVideo(Action cb = null)
    {
#if UNITY_EDITOR
        cb?.Invoke();
#else
        callback = cb;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }

#endif
    }

    void LoadDemandOnlyRewardedVideo()
    {
        Debug.Log("unity-script: LoadDemandOnlyRewardedVideoButtonClicked");
        IronSource.Agent.loadISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
    }

    void ShowDemandOnlyRewardedVideo()
    {
        Debug.Log("unity-script: ShowDemandOnlyRewardedVideoButtonClicked");
        if (IronSource.Agent.isISDemandOnlyRewardedVideoAvailable(REWARDED_INSTANCE_ID))
        {
            IronSource.Agent.showISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyRewardedVideoAvailable - False");
        }
    }

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
        if (canShowAd)
        {
        }
        else
        {
        }
    }

    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdStartedEvent()
    {
        //Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        //Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdClosedEvent()
    {
        callback?.Invoke();
    }
}
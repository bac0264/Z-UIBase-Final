using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIGachaLayout : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image iconGacha;
    [SerializeField] private Image iconCurrencyGacha1;
    [SerializeField] private Image iconCurrencyGacha10;

    [SerializeField] private Text priceGacha1;
    [SerializeField] private Text priceGacha10;
    [SerializeField] private Text timeGacha1;

    [SerializeField] private Button gachaFreeBtn;
    [SerializeField] private Button gacha1Btn;
    [SerializeField] private Button gacha10Btn;

    [SerializeField] private GachaData gachaData;


    private PlayerMoney playerMoney;
    private PlayerGacha playerGacha;
    
    private Coroutine timeCountDownGacha1 = null;
    private void Awake()
    {
        playerMoney = DataPlayer.GetModule<PlayerMoney>();
        playerGacha = DataPlayer.GetModule<PlayerGacha>();
        
        gacha1Btn.onClick.AddListener(OnClickGacha1);
        gacha10Btn.onClick.AddListener(OnClickGacha10);
        gachaFreeBtn.onClick.AddListener(OnClickFreeGacha);
    }

    public void UpdateView(GachaData gachaData)
    {
        this.gachaData = gachaData;

        iconCurrencyGacha1.sprite = LoadResourceController.GetMoneyIcon(gachaData.GetRequireGacha1().resId);
        iconCurrencyGacha10.sprite = LoadResourceController.GetMoneyIcon(gachaData.GetRequireGacha10().resId);
        
        priceGacha1.text = gachaData.GetRequireGacha1().resNumber.ToString();
        priceGacha10.text = gachaData.GetRequireGacha10().resNumber.ToString();
        timeGacha1.text = "";
        
        background.sprite = LoadResourceController.GetGachaBackground(gachaData.id);
        iconGacha.sprite = LoadResourceController.GetGachaIcon(gachaData.id);

        SetupFreeGacha();
    }

    public void OnClickGacha1()
    {
        var canGacha10 = gachaData != null && playerMoney.IsEnoughMoney(gachaData.GetRequireGacha1().GetResource());
        if (canGacha10)
        {
            OnSuccess(gachaData.GetGacha1());
        }
    }

    public void OnClickGacha10()
    {
        var canGacha10 = gachaData != null && playerMoney.IsEnoughMoney(gachaData.GetRequireGacha10().GetResource());
        if (canGacha10)
        {
            OnSuccess(gachaData.GetGacha10());
        }
    }

    public void OnClickFreeGacha()
    {
        playerGacha.SetLastTimeGacha1Free(gachaData.id, TimeManager.Ins.currentTime.TotalSecondTimeStamp());
        OnSuccess(gachaData.GetGachaFree());
    }
    
    private void OnSuccess(Reward[] rewards)
    {
        if (gachaData != null)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                rewards[i].RecieveReward();
            }

            UpdateView(gachaData);
            UIFrame.Instance.OpenWindow(WindowIds.UIShowReward, new ShowRewardProperties(rewards));
        }
    }
    
    private void SetupFreeGacha()
    {
        void UpdateView()
        {
            this.UpdateView(gachaData);
        }
        if (timeCountDownGacha1 != null)
        {
            CoroutineManager.instance.StopCoroutine(timeCountDownGacha1);
            timeCountDownGacha1 = null;
        }
        
        var timeCheck = TimeManager.Ins.check;
        var isFreeGacha = false;
        long rangeTime = 0;
        
        if (timeCheck)
        {
            rangeTime = playerGacha.GetRangeTime(gachaData.id, gachaData.timeConfig,
                TimeManager.Ins.currentTime.TotalSecondTimeStamp());
            isFreeGacha = rangeTime >= 0;
        }
        
        gachaFreeBtn.gameObject.SetActive(isFreeGacha);
        gacha1Btn.gameObject.SetActive(!isFreeGacha);
        timeGacha1.gameObject.SetActive(!isFreeGacha && timeCheck);
        
        if (!isFreeGacha && timeCheck)
        {
             timeCountDownGacha1 =
                     CoroutineManager.instance.StartCoroutine(TimeUtils.TimeGachaCoundown(timeGacha1, -rangeTime, UpdateView));
        }
    }
    
}
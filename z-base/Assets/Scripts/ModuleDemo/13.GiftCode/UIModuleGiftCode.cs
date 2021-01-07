using BestHTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GiftCode
{
    public class UIModuleGiftCode : AWindowController
    {
        private const int MAX_LENGTH = 40;
        private const int MIN_LENGTH = 0;
        
        [SerializeField] private InputField inputField;
        [SerializeField] private Button onClickBtn;

        protected override void Awake()
        {
            base.Awake();
            onClickBtn.onClick.AddListener(OnClickBtn);
        }

        public void OnClickBtn()
        {
            string giftCode = inputField.text;
            CheckGiftCode(giftCode, (result) =>
            {
                if (result)
                {
                    GiftCodeHttpClient.ClaimGiftCode(giftCode, OnClaimGiftCode);
                }
            });
        }

        private void OnClaimGiftCode(HTTPResponse response)
        {
            if (response != null)
            {
                if (response.StatusCode == HttpCode.SUCCESS)
                {
                    try
                    {
                        var rewardList = new List<Reward>();
                        GiftCodeReward reward= JsonConvert.DeserializeObject<GiftCodeReward>(response.DataAsText);
                        for (int i = 0; i < reward.items.Count; i++)
                        {
                            rewardList.Add(reward.items[i].GetReward());
                        }

                        var rewards = Reward.FixDuplicateRewards(rewardList);
                        UIFrame.Instance.OpenWindow(WindowIds.UIShowReward, new ShowRewardProperties(rewards));
                        
                    }
                    catch (Exception e)
                    {
                        //PopUpUtils.ShowNotifyError();
                    }
                }
                else
                {
                    Debug.Log("fail");
                }
                inputField.text = string.Empty;
            }
            else
            {
                Debug.Log("fail");
            }
        }

        private void CheckGiftCode(string giftCode, Action<bool> callback)
        {
            if (string.IsNullOrEmpty(giftCode))
            {

                if (callback != null)
                    callback(false);
                return;
            }
            if (giftCode.Length >= MIN_LENGTH && giftCode.Length <= MAX_LENGTH)
            {
                if (callback != null)
                    callback(true);
            }
            else
            {
                //PopUpUtils.ShowNotifyPopup(LocalizeUtils.GetText(LocalizeKey.REWARD_GIFT_CODE_LENGHT_INVALID));
                if (callback != null)
                    callback(false);
            }
        }
    }
}

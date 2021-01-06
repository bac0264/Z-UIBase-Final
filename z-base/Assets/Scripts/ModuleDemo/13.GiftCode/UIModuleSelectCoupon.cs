// using BestHTTP;
// using Data;
// using LocalData;
// using Newtonsoft.Json;
// using System;
// using System.Collections.Generic;
// using UI;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace GiftCode
// {
//     public class GiftCodeScene : SSController
//     {
//         public InputField inputField;
//
//         public void OnClickBtn()
//         {
//             string giftCode = inputField.text;
//             CheckGiftCode(giftCode, (result) =>
//             {
//                 if (result)
//                 {
//                     GameClient.Instance.giftCodeClient.ClaimGiftCode(giftCode, OnClaimGiftCode, true);
//                 }
//             });
//         }
//
//         private void OnClaimGiftCode(HTTPResponse response)
//         {
//             if (response != null)
//             {
//                 if (response.StatusCode == HttpCode.SUCCESS)
//                 {
//                     List<BaseItem> gifts = new List<BaseItem>();
//                     try
//                     {
//                         GiftCodeReward reward = JsonConvert.DeserializeObject<GiftCodeReward>(response.DataAsText);
//                         for (int i = 0; i < reward.items.Count; i++)
//                         {
//                             GiftCodeItem item = reward.items[i];
//
//                             switch ((ItemType)item.type)
//                             {
//                                 case ItemType.Rune:
//                                     {
//                                         var gift = new RuneItem(item);
//                                         gifts.Add(gift);
//                                         break;
//                                     }
//                                 case ItemType.SpecialItem:
//                                     {
//                                         var gift = SpecialItem.GetGifts(item);
//                                         gifts.AddRange(gift);
//                                         break;
//                                     }
//                                 default:
//                                     {
//                                         var gift = new BaseItem(item);
//                                         gifts.Add(gift);
//                                         break;
//                                     }
//                             }
//
//                         }
//                         RewardUtils.AddData(gifts, true, AnalyticsConstants.GIFT_CODE);
//
//                         if (reward.items.Count > 0)
//                         {
//                             WorldMapSettingScene.CallbackType = Gameplay.PopupCallbackType.ReloadWorldMap;
//                         }
//
//                         SSSceneManager.Instance.Close(false, () =>
//                         {
//                             PopUpUtils.ShowGiftPopup(gifts);
//                         });
//                     }
//                     catch (Exception e)
//                     {
//                         Log.Exception(e);
//                         PopUpUtils.ShowNotifyError();
//                     }
//                 }
//                 else
//                 {
//                     PopUpUtils.ShowNotifyError<GiftCodeServerMessageModel>(response.DataAsText);
//                 }
//                 inputField.text = string.Empty;
//             }
//         }
//
//         private void CheckGiftCode(string giftCode, Action<bool> callback)
//         {
//             if (string.IsNullOrEmpty(giftCode))
//             {
//
//                 if (callback != null)
//                     callback(false);
//                 return;
//             }
//             DataService.Instance.CheckVerifyVersion((result) =>
//             {
//                 if (result)
//                 {
//                     if (giftCode.Length >= ConfigStatics.GIFT_CODE_MIN_LENGTH && giftCode.Length <= ConfigStatics.GIFT_CODE_MAX_LENGTH)
//                     {
//                         if (callback != null)
//                             callback(true);
//                     }
//                     else
//                     {
//                         PopUpUtils.ShowNotifyPopup(LocalizeUtils.GetText(LocalizeKey.REWARD_GIFT_CODE_LENGHT_INVALID));
//                         if (callback != null)
//                             callback(false);
//                     }
//                 }
//                 else
//                 {
//                     if (callback != null)
//                         callback(false);
//                 }
//             });
//
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModuleSelectCoupon : MonoBehaviour
{
    public Button confirmButton;
    public InputField inputField;
    public Text couponStatus;
    

    public void SubmitCoupon()
    {
       // NetworkController.giftCodeHandler.ClaimGiftCode(inputField.text.Trim(), OnSubmitCoupon);
    }

    private void OnSubmitCoupon(GiftCodeResultInbound resultInbound)
    {
        if (resultInbound.logicCode == LogicCode.SUCCESS)
        {
            couponStatus.gameObject.SetActive(false);
            inputField.text = "";
            couponStatus.gameObject.SetActive(true);
        }
        else
        {
            couponStatus.gameObject.SetActive(true);
        }
    }
}

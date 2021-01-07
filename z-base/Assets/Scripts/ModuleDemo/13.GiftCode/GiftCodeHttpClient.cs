using BestHTTP;
using System;
using UnityEngine;

public class GiftCodeHttpClient
{
    public static string serverLocalUrl = "http://35.186.157.74:8011/giftcode/claim";

    public static void ClaimGiftCode(string giftCode, Action<HTTPResponse> callback)
    {
        
       CreatePOST(serverLocalUrl, callback, giftCode);
       
    }
    public static void CreatePOST(string url, Action<HTTPResponse> callback, string giftcode)
    {

        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (originalRequest, response) =>
        {
           // LogRequest(originalRequest, response);
            if (callback != null)
            {
              //  Debug.Log("response: "+response.DataAsText);
                callback(response);
            }
        });

        PlayerPrefs.SetInt("id", PlayerPrefs.GetInt("id") + 1);
        request.AddField("game_id", "6");
        request.AddField("login_provider", "0");
        request.AddField("user_id", "id: " +PlayerPrefs.GetInt("id"));
        request.AddField("code", giftcode.Trim());
       
        request.Send();
        
    }
}


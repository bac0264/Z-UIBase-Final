using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController; // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    private const string buyid25gem = "buyid25gem";

    [HideInInspector] public string nameIDproduct;
    public static IAPManager Instance;

    public Action callBack;

    public ConfigurationBuilder builder;
    //public static string kProductIDConsumable = "consumable";
    //public static string kProductIDNonConsumable = "nonconsumable";
    //public static string kProductIDSubscription = "subscription";
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void Buy(string id, Action callBack = null)
    {
        nameIDproduct = id;
#if UNITY_EDITOR
        callBack?.Invoke();
#else
        this.callBack = callBack;
        
        BuyProductID(id);
#endif
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        AddConsumableIds();
        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.

        //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);

        // Continue adding the non-consumable product.

        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 

        //builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
        //        { kProductNameAppleSubscription, AppleAppStore.Name },
        //        { kProductNameGooglePlaySubscription, GooglePlay.Name },
        //    });

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private void AddConsumableIds()
    {
        AddRawPackIds();
        AddBundleIds();
    }

    private void AddRawPackIds()
    {
        var rawCollection = LoadResourceController.GetShopRawPackCollection();
        var productIdList = rawCollection.GetShopRawPackProductIds();
        for (int i = 0; i < productIdList.Count; i++)
        {
            builder.AddProduct(productIdList[i], ProductType.Consumable);
        }
    }
    
    private void AddBundleIds()
    {
        var bundleCollection = LoadResourceController.GetShopBundleCollection();
        var productIdList = bundleCollection.GetShopBundleProductIds();
        for (int i = 0; i < productIdList.Count; i++)
        {
            builder.AddProduct(productIdList[i], ProductType.Consumable);
        }
    }
    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result +
                          ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (nameIDproduct != null)
        {
            if (String.Equals(args.purchasedProduct.definition.id, nameIDproduct, StringComparison.Ordinal))
            {
                BuyCompleted(nameIDproduct);
            }
        }

        //else
        //{
        //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        //}

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    public void BuyCompleted(string _nameid)
    {
        callBack?.Invoke();
    }
}
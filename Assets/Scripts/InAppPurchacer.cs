using System;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class InAppPurchacer : MonoBehaviour, IStoreListener
{
    public static InAppPurchacer instance;
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string twentyTooltips = "20tip";
    public static string fiftyTooltips = "50tip";
    public static string hundredTooltips = "100tip";
    public static string noADS = "no_ads";

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {

        if (IsInitialized())
        {
            return;
        }
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(twentyTooltips, ProductType.Consumable);
        builder.AddProduct(fiftyTooltips, ProductType.Consumable);
        builder.AddProduct(hundredTooltips, ProductType.Consumable);
        builder.AddProduct(noADS, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
       
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void Buy10Tips()
    {
        BuyProductID(twentyTooltips);
    }

    public void Buy25Tips()
    {
        BuyProductID(fiftyTooltips);
    }

    public void Buy50Tips()
    {
        BuyProductID(hundredTooltips);
    }

    public void BuyNoADS()
    {
        BuyProductID(noADS);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
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
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, twentyTooltips, StringComparison.Ordinal))
        {
            int tips = PlayerPrefs.GetInt("Hints", 0);
            PlayerPrefs.SetInt("Hints", tips + 10);
            Debug.Log("You just bought 20tips");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, fiftyTooltips, StringComparison.Ordinal))
        {
            int tips = PlayerPrefs.GetInt("Hints", 0);
            PlayerPrefs.SetInt("Hints", tips + 25);
            Debug.Log("You just bought 50tips");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, hundredTooltips, StringComparison.Ordinal))
        {
            int tips = PlayerPrefs.GetInt("Hints", 0);
            PlayerPrefs.SetInt("Hints", tips + 50);
            Debug.Log("You just bought 100tips");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, noADS, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("ADSRemoved", 1);
            Debug.Log("You removed your ads");
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}

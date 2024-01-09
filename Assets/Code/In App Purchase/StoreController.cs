using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Unity.Services.Core.Environments;
using System.Linq;

public class StoreController : MonoBehaviour, IDetailedStoreListener
{
    public GameObject PurchasingMask;
    public GameObject generalControllers;
    public List<GameObject> StoreItems;
    private IStoreController storeController;
    private IExtensionProvider storeExtensions;
    private Button purchaseButton;

    public AudioSource successfulPurchaseSound;
    public TMP_Text testConsoleOutput;

    void Start()
    {
        //oyun a��l�rken initial yapaca��z. ��nk� restore buton i�in loadcatalog'un �al��m�� olmas� ve OnInitialized fonksiyonuna girmi� olmas� gerekiyor
        //UnityServicesInitial();
    }

    //rubik oyun mant��� gere�i initialize ba�ka yerde ba�lat�l�yor. Bu y�zden servis ba�latma fonskiyonunu kullanm�yoruz.
    public async void UnityServicesInitial()
    {
        
        if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
            await UnityServices.InitializeAsync();

        LoadCatalog();
    }

    void LoadCatalog()
    {
        try
        {
            ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((Resources.LoadAsync<TextAsset>("IAPProductCatalog").asset as TextAsset).text);

            //StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.DeveloperUser;
            //StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                builder.AddProduct(item.id, item.type);
            }

            UnityPurchasing.Initialize(this, builder);
        }
        catch (Exception e)
        {

            Debug.Log("Hata: StoreController LoadCatalog hata olu�tu. " + e.Message);
        }

    }

    //IAP ba�ar�l� �ekilde ba�lat�ld���nda girer
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        try
        {
            this.storeController = controller;
            this.storeExtensions = extensions;
            foreach (GameObject item in StoreItems)
            {
                //ProductCatalogItem itemData = catalog.allProducts.Where(i => i.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();

                Product product_ = storeController.products.all.Where(i => i.definition.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();
                item.GetComponent<StoreItem>().SetItemMetaData(product_.metadata.localizedTitle, product_.metadata.localizedDescription, product_.metadata.localizedPriceString + " " + product_.metadata.isoCurrencyCode);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Hata: StoreController OnInitialized hata olu�tu. " + e.Message);

        }
    }

    //IAP ba�lat�lmas�nda hata ile kar��la��l�rsa
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP ba�latma hatas�. OnInitializeFailed: " + error);
        throw new System.NotImplementedException();
    }

    //IAP ba�lat�lmas�nda hata ile kar��la��l�rsa
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}. \r\nError message: {message}");
    }

    //Sat�n almada hata meydana gelirse
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //sat�n alma ba�ar�l� olursa
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);

        //do something, like give the player their currency, unlock the item 
        //update some metrics or analytics, etc...

        ApplyPurchasedItemEffect(purchaseEvent.purchasedProduct.definition.id, purchaseEvent.purchasedProduct.metadata.localizedDescription);

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productId, Button button)
    {
        purchaseButton = button;
        if (purchaseButton.IsActive())
        {
            purchaseButton.enabled = false;
        }
        PurchasingMask.SetActive(true);
        Product product = storeController.products.all.Where(i => i.definition.id == productId).FirstOrDefault();
        storeController.InitiatePurchase(product);
    }

    private void PurchaseCompleted()
    {
        try
        {
            if (purchaseButton.IsActive())
            {
                purchaseButton.enabled = true;
            }
        }
        catch (Exception e)
        {

            Debug.Log("PurchaseCompleted hata. PurchaseCompleted: " + e.Message);
        }

    }

    public void RestorePurchase()
    {
        //oyun a��l�rken initial ve loadcatalog i�lemleri yap�l�yor.
       // if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
       //     await UnityServices.InitializeAsync();

       //LoadCatalog();

        if (storeController != null)
        {
#if UNITY_ANDROID
            Product product_noAds = storeController.products.all.Where(i => i.definition.id == "no_ads").FirstOrDefault();
#elif UNITY_IOS
            Product product_noAds = storeController.products.all.Where(i => i.definition.id == "kubik_no_ads").FirstOrDefault();
#endif
            if (product_noAds != null)
            {
                if (product_noAds.hasReceipt)
                {
                    PlayerPrefs.SetString("NoAdsActive", "1");
                    PlayerPrefs.Save();

                    Debug.Log("Daha �nce sat�n ald���n�z reklam yok �r�n� geri getirildi.");
                }
                else
                {
                    Debug.Log("Reklam yok �r�n� sat�n al�nmam��");
                }
            }

#if UNITY_ANDROID
            Product product_increaseHint = storeController.products.all.Where(i => i.definition.id == "increase_hint").FirstOrDefault();
#elif UNITY_IOS
            Product product_increaseHint = storeController.products.all.Where(i => i.definition.id == "kubik_increase_hint").FirstOrDefault();
#endif
            if (product_increaseHint != null)
            {
                if (product_increaseHint.hasReceipt)
                {
                    PlayerPrefs.SetString("increaseHintPowerupActive", "1");
                    PlayerPrefs.Save();

                    GlobalVariable.defaultSolvingQuantity = 15;
                    PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
                    PlayerPrefs.Save();

                    Debug.Log("Daha �nce sat�n ald���n�z ipucu miktar� artt�rma �r�n� geri getirildi.");
                }
                else
                {
                    Debug.Log("�pucu miktar� artt�rma �r�n� sat�n al�nmam��");
                }
            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

        Debug.Log("Sat�n almada hata meydana geldi. message:" + failureDescription.message + " reason:" + failureDescription.reason + " �d:" + failureDescription.productId);

        if (product.hasReceipt)
        {
            Debug.Log("Daha �nce sat�n al�nm��. id:" + product.definition.id);
            ApplyPurchasedItemEffect(product.definition.id, product.metadata.localizedDescription);
        }

        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //ma�azadan bir �r�n sat�n al�nd���nda oyuna nas�l etki edecekse ayarlamalar yap�l�r.
    void ApplyPurchasedItemEffect(string itemId, string description)
    {
#if UNITY_ANDROID
        if (itemId == "no_ads")
        {
            PlayerPrefs.SetString("NoAdsActive", "1");
            PlayerPrefs.Save();

        }
        else if (itemId == "joker_easy1")
        {
            string easyJokerQuantity = PlayerPrefs.GetString("EasyJoker");
            if (easyJokerQuantity == "")
            {
                easyJokerQuantity = description;
                PlayerPrefs.SetString("EasyJoker", easyJokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(easyJokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("EasyJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId == "joker_very_easy1")
        {
            string veryEasyJokerQuantity = PlayerPrefs.GetString("VeryEasyJoker");
            if (veryEasyJokerQuantity == "")
            {
                veryEasyJokerQuantity = description;
                PlayerPrefs.SetString("VeryEasyJoker", veryEasyJokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(veryEasyJokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("VeryEasyJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId == "increase_hint")
        {
            PlayerPrefs.SetString("increaseHintPowerupActive", "1");
            PlayerPrefs.Save();

            GlobalVariable.defaultSolvingQuantity = 15;
            PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
            PlayerPrefs.Save();
        }
#elif UNITY_IOS
        if (itemId == "kubik_no_ads")
        {
            PlayerPrefs.SetString("NoAdsActive", "1");
            PlayerPrefs.Save();

        }
        else if (itemId == "kubik_joker_easy1")
        {
            string easyJokerQuantity = PlayerPrefs.GetString("EasyJoker");
            if (easyJokerQuantity == "")
            {
                easyJokerQuantity = description;
                PlayerPrefs.SetString("EasyJoker", easyJokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(easyJokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("EasyJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId == "kubik_joker_very_easy1")
        {
            string veryEasyJokerQuantity = PlayerPrefs.GetString("VeryEasyJoker");
            if (veryEasyJokerQuantity == "")
            {
                veryEasyJokerQuantity = description;
                PlayerPrefs.SetString("VeryEasyJoker", veryEasyJokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(veryEasyJokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("VeryEasyJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId == "kubik_increase_hint")
        {
            PlayerPrefs.SetString("increaseHintPowerupActive", "1");
            PlayerPrefs.Save();

            GlobalVariable.defaultSolvingQuantity = 15;
            PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
            PlayerPrefs.Save();
        }
#endif
        successfulPurchaseSound.Play();
    }
}

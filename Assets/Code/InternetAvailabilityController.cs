using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InternetAvailabilityController : MonoBehaviour
{
    public GameObject storeController;
    public GameObject mainMenuNoAdsButton;
    public GameObject InternetRequiredPopup;

    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 5.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {

            GlobalVariable.internetAvaible = false;
            InternetRequiredPopup.SetActive(true);
        }
        else
        {
            GlobalVariable.internetAvaible = true;
            InternetRequiredPopup.SetActive(false);
        }
        IAPItemsVisibleControl();
    }

    void IAPItemsVisibleControl()
    {
        if (GlobalVariable.internetAvaible == false)
        {
            foreach (GameObject storeItem in storeController.GetComponent<StoreController>().StoreItems)
            {
                storeItem.SetActive(false);
                mainMenuNoAdsButton.SetActive(false);
            }
        }
        else if (GlobalVariable.internetAvaible == true && PlayerPrefs.GetString("NoAdsActive") != "1")
        {
            foreach (GameObject storeItem in storeController.GetComponent<StoreController>().StoreItems)
            {
                storeItem.SetActive(true);
                mainMenuNoAdsButton.SetActive(true);
            }
        }

    }
}

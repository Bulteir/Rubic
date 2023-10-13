using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdMobRewardedAdController : MonoBehaviour
{
    /// <summary>
    /// UI element activated when an ad is ready to show.
    /// </summary>
    public GameObject AdLoadedStatus;
    public GameObject easyButton;
    public GameObject veryEasyButton;

    [Tooltip("Reklam�n para kazand�rd��� tahmin edildi�inde tektiklenir.")]
    public UnityEvent OnAdPaidEvent;
    [Tooltip("Reklam g�sterim/impression elde etti�inde tetiklenir.")]
    public UnityEvent OnAdImpressionRecordedEvent;
    [Tooltip("Reklam t�kland���nda tetiklenir.")]
    public UnityEvent OnAdClickedEvent;
    [Tooltip("Reklam g�sterildi�inde tetiklenir.")]
    public UnityEvent OnAdFullScreenContentOpenedEvent;
    [Tooltip("Reklam kapand���nda tetiklenir.")]
    public UnityEvent OnAdFullScreenContentClosedEvent;
    [Tooltip("Reklam a��l�rken hata ald���nda tetiklenir.")]
    public UnityEvent OnAdFullScreenContentFailedEvent;

#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3475441178822227/9510898940";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3475441178822227/5888183825";
#else
        private const string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            //Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            _rewardedAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.
            if (AdLoadedStatus != null)
                AdLoadedStatus?.SetActive(true);

            easyButton.GetComponent<NewGameMenu_EasyJoker_Btn>().adLoaded();
            veryEasyButton.GetComponent<NewGameMenu_VeryEasyJoker_Btn>().adLoaded();
        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                //Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                //                        reward.Amount,
                //                        reward.Type));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.
        if (AdLoadedStatus != null)
            AdLoadedStatus?.SetActive(false);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        // Inform the UI that the ad is not ready.
        if (AdLoadedStatus != null)
            AdLoadedStatus?.SetActive(false);
    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogResponseInfo()
    {
        if (_rewardedAd != null)
        {
            var responseInfo = _rewardedAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            //Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
            //    adValue.Value,
            //    adValue.CurrencyCode));
            OnAdPaidEvent.Invoke();
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            //Debug.Log("Rewarded ad recorded an impression.");
            OnAdImpressionRecordedEvent.Invoke();
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            //Debug.Log("Rewarded ad was clicked.");
            OnAdClickedEvent.Invoke();
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            //Debug.Log("Rewarded ad full screen content opened.");
            OnAdFullScreenContentOpenedEvent.Invoke();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            //Debug.Log("Rewarded ad full screen content closed.");
            OnAdFullScreenContentClosedEvent.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //Debug.LogError("Rewarded ad failed to open full screen content with error : "
            //    + error);
            OnAdFullScreenContentFailedEvent.Invoke();
        };
    }
}

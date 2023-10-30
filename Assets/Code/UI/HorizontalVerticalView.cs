using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HorizontalVerticalView : MonoBehaviour
{
    public Camera cam;
    public GameObject camPortraitPrefab;
    public GameObject camLandscapePrefab;

    bool isOrientationPortrait;
    IEnumerator BannerRequest;

    void Start()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            isOrientationPortrait = false;
            setLandscapeSettings();
        }
        else
        {
            isOrientationPortrait = true;
            setPortraitSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) && isOrientationPortrait == true)
        {
            isOrientationPortrait = false;
            setLandscapeSettings();
            if (GlobalVariable.gameState == GlobalVariable.gameState_inGame) // banner reklamýnýn telefon döndürüldüðünde tekrar çaðrýlmasý için kullanýlacak
            {
                transform.GetComponent<AdMobBannerViewController>().DestroyAd();
                if (BannerRequest != null)
                {
                    StopCoroutine(BannerRequest);
                }                
                BannerRequest = RequestBannner();
                StartCoroutine(BannerRequest);
            }
        }
        else if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isOrientationPortrait == false)
        {
            isOrientationPortrait = true;
            setPortraitSettings();
            if (GlobalVariable.gameState == GlobalVariable.gameState_inGame) // banner reklamýnýn telefon döndürüldüðünde tekrar çaðrýlmasý için kullanýlacak
            {
                transform.GetComponent<AdMobBannerViewController>().DestroyAd();
                if (BannerRequest != null)
                {
                    StopCoroutine(BannerRequest);
                }
                BannerRequest = RequestBannner();
                StartCoroutine(BannerRequest);
            }
        }
    }

    void setLandscapeSettings ()
    {
        cam.transform.position = camLandscapePrefab.transform.position;
        cam.transform.rotation = camLandscapePrefab.transform.rotation;
        cam.GetComponent<Camera>().fieldOfView = camLandscapePrefab.GetComponent<Camera>().fieldOfView;
    }

    void setPortraitSettings ()
    {
        cam.transform.position = camPortraitPrefab.transform.position;
        cam.transform.rotation = camPortraitPrefab.transform.rotation;
        cam.GetComponent<Camera>().fieldOfView = camPortraitPrefab.GetComponent<Camera>().fieldOfView;
    }

    IEnumerator RequestBannner()
    {
        yield return new WaitForSeconds(5);
        if (PlayerPrefs.GetString("NoAdsActive") != "1")
        {
            transform.GetComponent<AdMobBannerViewController>().LoadAd();
        }
        BannerRequest = null;
    }
}

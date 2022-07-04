using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Presets;

public class HorizontalVerticalView : MonoBehaviour
{
    public Camera cam;
    public GameObject counter;
    public GameObject shuffle_btn;
    public GameObject camPortraitPrefab;
    public GameObject camLandscapePrefab;
    public GameObject counterPortraitPrefab;
    public GameObject counterLandscapePrefab;
    public GameObject shuffle_btnPortraitPrefab;
    public GameObject shuffle_btnLandscapePrefab;
    bool isOrientationPortrait;

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
        }
        else if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isOrientationPortrait == false)
        {
            isOrientationPortrait = true;
            setPortraitSettings();
        }
    }

    void setLandscapeSettings ()
    {
        cam.transform.position = camLandscapePrefab.transform.position;
        cam.transform.rotation = camLandscapePrefab.transform.rotation;
        cam.GetComponent<Camera>().fieldOfView = camLandscapePrefab.GetComponent<Camera>().fieldOfView;
        counter.GetComponent<RectTransform>().anchoredPosition = counterLandscapePrefab.GetComponent<RectTransform>().anchoredPosition;
        shuffle_btn.GetComponent<RectTransform>().anchoredPosition = shuffle_btnLandscapePrefab.GetComponent<RectTransform>().anchoredPosition;
        gameObject.GetComponent<MenuControl>().uiScaleOptimizer();
    }

    void setPortraitSettings ()
    {
        cam.transform.position = camPortraitPrefab.transform.position;
        cam.transform.rotation = camPortraitPrefab.transform.rotation;
        cam.GetComponent<Camera>().fieldOfView = camPortraitPrefab.GetComponent<Camera>().fieldOfView;
        counter.GetComponent<RectTransform>().anchoredPosition = counterPortraitPrefab.GetComponent<RectTransform>().anchoredPosition;
        shuffle_btn.GetComponent<RectTransform>().anchoredPosition = shuffle_btnPortraitPrefab.GetComponent<RectTransform>().anchoredPosition;
        gameObject.GetComponent<MenuControl>().uiScaleOptimizer();
    }
}

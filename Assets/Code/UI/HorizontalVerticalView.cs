using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Presets;

public class HorizontalVerticalView : MonoBehaviour
{
    public Camera cam;
    public GameObject camPortraitPrefab;
    public GameObject camLandscapePrefab;

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
    }

    void setPortraitSettings ()
    {
        cam.transform.position = camPortraitPrefab.transform.position;
        cam.transform.rotation = camPortraitPrefab.transform.rotation;
        cam.GetComponent<Camera>().fieldOfView = camPortraitPrefab.GetComponent<Camera>().fieldOfView;
    }
}

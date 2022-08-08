using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;
using UnityEngine.UI;
using TMPro;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;


public class Solve_Btn : MonoBehaviour
{
    public List<Transform> faceDetectors;
    public Transform rubicCube;
    public Button solve_Btn;
    public bool isSolvingStepActive = false;
    public bool isDoubleSolvingStep = false;
    public Button SolvingQuantity_Btn;
    public GameObject GeneralControls;

    //küpümüzün up=> beyaz
    //küpümüzün front=> mavi
    //küpümüzün left=> sarý
    //küpümüzün right=> kýrmýzý
    //küpümüzün back=> turuncu
    //küpümüzün down=> yeþil
    //search string yazýlma sýrasý Up,Right,Front,Down,Left,Back

    public void onClick()
    {
        int solvingQuantity = PlayerPrefs.GetInt("SolvingQuantity");

        if (rubicCube.GetComponent<CubeControl>().isRotateStarted == false && solvingQuantity > 0)
        {
            solve_Btn.interactable = false;
            isSolvingStepActive = true;
            SetSolvingQuantityBtnInteractable(false);
            solvingQuantity--;
            PlayerPrefs.SetInt("SolvingQuantity", solvingQuantity);
            PlayerPrefs.Save();
            SolvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = solvingQuantity.ToString();

            string solution = getSolution();
            if (solution.Contains("Error"))
            {
                PlayerPrefs.DeleteKey("Solution");
                StartCoroutine(shuffleOneStepForEveryTouch(solution));
            }
            else
            {
                rubicCube.GetComponent<CubeControl>().rotateAndFixCubeForKociembaStart(faceDetectors, transform);
            }

            if (solvingQuantity == 1)//son bir tane hak kalýnca reklam istiyoruz.
                GeneralControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();

        }
        else if (solvingQuantity == 0)
        {
            GeneralControls.GetComponent<AdMobController>().ShowRewardedAd();//Reklamý gösteriyoruz.
        }
    }

    IEnumerator shuffleOneStepForEveryTouch(string solution)
    {
        int orjinalShuffleCount = rubicCube.GetComponent<CubeControl>().shuffleStepCount;
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = 1;
        rubicCube.GetComponent<CubeControl>().shuffleCube(solve_Btn);
        yield return new WaitUntil(() => rubicCube.GetComponent<CubeControl>().isRotateStarted == false);
        solution = getSolution();
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = orjinalShuffleCount;
    }

    public string getSolution()
    {
        string searchString = "";
        foreach (var item in faceDetectors)
        {
            searchString += item.GetComponent<CubeFaceColorDetect>().getSearchSubStrings();
        }

        string info = "";
        string solution = SearchRunTime.solution(searchString, out info);
        return solution;
    }

    public void SetSolvingQuantityBtnInteractable (bool interactable)
    {
        if(interactable)
        {
            SolvingQuantity_Btn.image.color = new Color(255f/255f, 0, 0, 255f/255f);
        }
        else
        {
            SolvingQuantity_Btn.image.color = new Color(255f / 255f, 0, 0, 128f / 255f);
        }
    }

    public void SuccesedRewardedAd()
    {
        int solvingQuantity = PlayerPrefs.GetInt("SolvingQuantity");
        solvingQuantity = GlobalVariable.defaultSolvingQuantity;
        PlayerPrefs.SetInt("SolvingQuantity", solvingQuantity);
        PlayerPrefs.Save();
        SolvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = solvingQuantity.ToString();
    }
}

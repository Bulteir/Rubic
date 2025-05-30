using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;


public class Solve_Btn : MonoBehaviour
{
    public List<Transform> faceDetectors;
    public Transform rubicCube;
    public Button solve_Btn;
    public TMP_Text solve_Btn_Text;
    public bool isSolvingStepActive = false;
    public bool isDoubleSolvingStep = false;
    public Button SolvingQuantity_Btn;
    public GameObject GeneralControls;
    public GameObject Counter;

    //k�p�m�z�n up=> beyaz
    //k�p�m�z�n front=> mavi
    //k�p�m�z�n left=> sar�
    //k�p�m�z�n right=> k�rm�z�
    //k�p�m�z�n back=> turuncu
    //k�p�m�z�n down=> ye�il
    //search string yaz�lma s�ras� Up,Right,Front,Down,Left,Back

    public void onClick()
    {
        rubicCube.parent.GetComponent<CubeCameraControl>().isEnteredSolveButton = true;

        if (PlayerPrefs.GetString("NoAdsActive") == "1" && rubicCube.GetComponent<CubeControl>().isRotateStarted == false)
        {
            solve_Btn.interactable = false;
            isSolvingStepActive = true;
            SetSolvingQuantityBtnInteractable(false);

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
            AddTimeToCounter();
        }
        else
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

                if (solvingQuantity == 1)//son bir tane hak kal�nca reklam istiyoruz.
                {
                    GeneralControls.GetComponent<AdMobRewardedAdController>().LoadAd();             
                }
                AddTimeToCounter();

            }
            else if (solvingQuantity == 0)
            {
                GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_solve;
                GeneralControls.GetComponent<AdMobRewardedAdController>().ShowAd();//Reklam� g�steriyoruz.
            }
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
        rubicCube.GetComponent<CubeControl>().resolveMoves++;
        rubicCube.GetComponent<CubeControl>().Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + rubicCube.GetComponent<CubeControl>().resolveMoves;
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

    public void SetSolvingQuantityBtnInteractable(bool interactable)
    {
        if (interactable)
        {
            SolvingQuantity_Btn.image.color = new Color(255f / 255f, 0, 0, 255f / 255f);
            solve_Btn_Text.GetComponent<TMP_Text>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            SolvingQuantity_Btn.image.color = new Color(255f / 255f, 0, 0, 128f / 255f);
            solve_Btn_Text.GetComponent<TMP_Text>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
        }
    }

    public void SuccesedRewardedAd()
    {
        if (GlobalVariable.rewardAdState == GlobalVariable.rewardAdState_solve)
        {
            int solvingQuantity = PlayerPrefs.GetInt("SolvingQuantity");
            solvingQuantity = GlobalVariable.defaultSolvingQuantity;
            PlayerPrefs.SetInt("SolvingQuantity", solvingQuantity);
            PlayerPrefs.Save();
            SolvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = solvingQuantity.ToString();
        }
    }

    //�zellikle no ads e�yas� al�n�nca ipucu butonuna seri �ekilde bas�lmas� ile rubik k�p h�zl� bir �ekilde ��z�lebiliyor. Bu da leaderboardda hile gibi oluyor.
    //Bunu engellemek i�in ipucu butonuna bas�ld���nda sayaca 30 saniye ekleyelim.
    void AddTimeToCounter()
    {
        if (Counter.GetComponent<Counter>().isChallengeModeActive == false)
        {
            //30 saniye
            Counter.GetComponent<Counter>().count += 30;
        }

    }
}

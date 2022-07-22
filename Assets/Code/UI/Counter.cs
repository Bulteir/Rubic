using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class Counter : MonoBehaviour
{
    TMP_Text text;
    float count;
    float miliSecond;
    int second;
    int minute;
    int hour;

    bool isStarted = false;
    public bool isChallengeModeActive = false;

    public GameObject rubicCube;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted == true && GlobalVariable.gameState == GlobalVariable.gameState_inGame && isChallengeModeActive == false)
        {
            count += Time.deltaTime;
            miliSecond = (count % 1) * 100;
            second = (int)count % 60;
            minute = ((int)count / 60) % 60;
            hour = ((int)count / 3600) % 24;

            if (hour == 0)
                text.text = string.Format("{0:00}:{1:00}:{2:00}", minute, second, miliSecond);
            else
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hour, minute, second, miliSecond);
        }
        else if (isStarted == true && isChallengeModeActive == true && GlobalVariable.gameState == GlobalVariable.gameState_inGame)
        {
            miliSecond = (count % 1) * 100;
            second = (int)count % 60;
            minute = ((int)count / 60) % 60;
            hour = ((int)count / 3600) % 24;

            if (hour == 0)
                text.text = string.Format("{0:00}:{1:00}:{2:00}", minute, second, miliSecond);
            else
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hour, minute, second, miliSecond);
            count -= Time.deltaTime;
            if (count < 0)
            {
                isStarted = false;
                GlobalVariable.gameState = GlobalVariable.gameState_TimesUp;
            }
        }
    }

    public void startCounter()
    {
        isStarted = true;
    }

    public void pauseCounter()
    {
        isStarted = false;
    }

    public void resetCounter()
    {

        if (isChallengeModeActive == false)
        {
            count = 0;
            text.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
        }
        else
        {
            string json = PlayerPrefs.GetString("Bests");
            List<CubeControl.BestTimesStruct> bestTimesList = new List<CubeControl.BestTimesStruct>();
            bestTimesList = JsonUtility.FromJson<CubeControl.JsonableListWrapper<CubeControl.BestTimesStruct>>(json).list;

            text.text = bestTimesList[0].time;
            SetCounterForChallengeMode(bestTimesList[0].time);
        }
        isStarted = false;

        rubicCube.GetComponent<CubeControl>().resolveMoves = 0;
        rubicCube.GetComponentInChildren<CubeControl>().Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") +
            rubicCube.GetComponentInChildren<CubeControl>().resolveMoves;
    }

    void SetCounterForChallengeMode(string bestTime)
    {
        string[] parsedTime = bestTime.Split(":");

        miliSecond = int.Parse(parsedTime[parsedTime.Length - 1]);
        second = int.Parse(parsedTime[parsedTime.Length - 2]);
        minute = int.Parse(parsedTime[parsedTime.Length - 3]);

        count = (miliSecond / 100f) + (second) + (minute * 60);


        if (parsedTime.Length > 3)
        {
            hour = int.Parse(parsedTime[parsedTime.Length - 4]);
            count += hour * 3600;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class Counter : MonoBehaviour
{
    public TMP_Text text;
    float count;
    float miliSecond;
    int second;
    int minute;
    int hour;

    bool isStarted = false;
    public bool isChallengeModeActive = false;

    public GameObject rubicCube;

    // Update is called once per frame
    void Update()
    {
        if (isStarted == true && isChallengeModeActive == false && GlobalVariable.gameState == GlobalVariable.gameState_inGame)
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

    float GetTotalDeltaTimeFromBestTime(string time)
    {
        string[] parsedTime = time.Split(":");

        int _miliSecond = int.Parse(parsedTime[parsedTime.Length - 1]);
        int _second = int.Parse(parsedTime[parsedTime.Length - 2]);
        int _minute = int.Parse(parsedTime[parsedTime.Length - 3]);

        float _count = (_miliSecond / 100f) + (_second) + (_minute * 60);

        if (parsedTime.Length > 3)
        {
            int _hour = int.Parse(parsedTime[parsedTime.Length - 4]);
            _count += _hour * 3600;
        }
        return _count;
    }

    public string GetDifferenceTwoTimes(string time1, string time2)
    {
        string result = "";
        float time1DeltaTime = GetTotalDeltaTimeFromBestTime(time1);
        float time2DeltaTime = GetTotalDeltaTimeFromBestTime(time2);

        float _count = time1DeltaTime - time2DeltaTime;
        float _miliSecond = (_count % 1) * 100;
        int _second = (int)_count % 60;
        int _minute = ((int)_count / 60) % 60;
        int _hour = ((int)_count / 3600) % 24;

        if (hour == 0)
            result = string.Format("{0:00}:{1:00}:{2:00}", _minute, _second, _miliSecond);
        else
            result = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _hour, _minute, _second, _miliSecond);

        return result;
    }
}

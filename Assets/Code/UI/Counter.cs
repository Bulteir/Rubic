using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Counter : MonoBehaviour
{
    TMP_Text text;
    float count;
    float miliSecond;
    int second;
    int minute;
    int hour;

    bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        //startCounter();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted == true && GlobalVariable.gameState == GlobalVariable.gameState_inGame)
        {
            count += Time.deltaTime;
            miliSecond = (count % 1) * 1000;
            second = (int)count % 60;
            minute = ((int)count / 60) % 60;
            hour = ((int)count / 3600) % 24;

            if (hour == 0)
                text.text = string.Format("{0:00}:{1:00}:{2:000}", minute, second, miliSecond);
            else
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", hour, minute, second, miliSecond);
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
        count = 0;
        text.text = string.Format("{0:00}:{1:00}:{2:000}", 0, 0, 0);
        isStarted = false;
    }
}

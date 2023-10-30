using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu_VeryEasyJoker_Btn : MonoBehaviour
{
    public Transform rubicCube;
    public GameObject generalControls;
    public Button jokerQuantity;
    public Button veryEasyButton;
    bool adIsReady = false;
    public void onClick()
    {
        if (GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_veryEasyJoker && GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_easyJoker)
        {

            if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                veryEasyButton.interactable = false;
                rubicCube.GetComponent<CubeControl>().shuffleStepCount = 5;
            }
            else
            {
                GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_veryEasyJoker;
                int veryEasyJokerQuantity = int.Parse(PlayerPrefs.GetString("VeryEasyJoker"));
                if (veryEasyJokerQuantity > 0)
                {
                    veryEasyButton.interactable = false;
                    veryEasyJokerQuantity--;
                    PlayerPrefs.SetString("VeryEasyJoker", veryEasyJokerQuantity.ToString());
                    PlayerPrefs.Save();
                    rubicCube.GetComponent<CubeControl>().shuffleStepCount = 5;
                }
                else if (adIsReady)
                {
                    adIsReady = false;
                    generalControls.GetComponent<AdMobRewardedAdController>().ShowAd();
                    generalControls.GetComponent<AdMobRewardedAdController>().LoadAd();

                }
                jokerQuantity.GetComponentInChildren<TMPro.TMP_Text>().text = veryEasyJokerQuantity.ToString();
            }
           
        }
    }

    public void SetVeryEasyJokerQuantity()
    {
        if (GlobalVariable.rewardAdState == GlobalVariable.rewardAdState_veryEasyJoker)
        {
            veryEasyButton.interactable = true;
            GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_idle;
            PlayerPrefs.SetString("VeryEasyJoker", "1");
            PlayerPrefs.Save();
            jokerQuantity.GetComponentInChildren<TMPro.TMP_Text>().text = PlayerPrefs.GetString("VeryEasyJoker");
        }
    }

    public void adFailed()
    {
        StartCoroutine(requestAdAgain());
    }

    IEnumerator requestAdAgain()
    {
        yield return new WaitForSeconds(5);
        veryEasyButton.interactable = true;
        GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_idle;
        generalControls.GetComponent<AdMobRewardedAdController>().LoadAd();

    }
    public void adLoaded()
    {
        adIsReady = true;
    }
}

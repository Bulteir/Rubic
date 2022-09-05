using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu_EasyJoker_Btn : MonoBehaviour
{
    public Transform rubicCube;
    public GameObject generalControls;
    public Button jokerQuantity;
    public Button easyButton;
    bool adIsReady = false;
    public void onClick()
    {
        if (GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_veryEasyJoker && GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_easyJoker)
        {
            GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_easyJoker;
            int easyJokerQuantity = int.Parse(PlayerPrefs.GetString("EasyJoker"));
            if (easyJokerQuantity > 0)
            {
                easyButton.interactable = false;
                easyJokerQuantity--;
                PlayerPrefs.SetString("EasyJoker", easyJokerQuantity.ToString());
                PlayerPrefs.Save();
                rubicCube.GetComponent<CubeControl>().shuffleStepCount = 10;
            }
            else if (adIsReady)
            {
                adIsReady = false;
                generalControls.GetComponent<AdMobController>().ShowRewardedAd();
                generalControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
            }
            jokerQuantity.GetComponentInChildren<TMPro.TMP_Text>().text = easyJokerQuantity.ToString();
        }
    }

    public void SetEasyJokerQuantity()
    {
        if (GlobalVariable.rewardAdState == GlobalVariable.rewardAdState_easyJoker)
        {
            easyButton.interactable = true;
            GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_idle;
            PlayerPrefs.SetString("EasyJoker", "3");
            PlayerPrefs.Save();
            jokerQuantity.GetComponentInChildren<TMPro.TMP_Text>().text = PlayerPrefs.GetString("EasyJoker");
        }
    }

    public void adFailed()
    {
        easyButton.interactable = true;
        GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_idle;
        generalControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
    }

    public void adLoaded()
    {
        adIsReady = true;
    }
}

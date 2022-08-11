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

    public void onClick()
    {
        if (GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_veryEasyJoker && GlobalVariable.rewardAdState != GlobalVariable.rewardAdState_easyJoker)
        {
            veryEasyButton.interactable = false;
            GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_veryEasyJoker;

            int veryEasyJokerQuantity = int.Parse(PlayerPrefs.GetString("VeryEasyJoker"));
            if (veryEasyJokerQuantity > 0)
            {
                veryEasyJokerQuantity--;
                PlayerPrefs.SetString("VeryEasyJoker", veryEasyJokerQuantity.ToString());
                PlayerPrefs.Save();
                rubicCube.GetComponent<CubeControl>().shuffleStepCount = 5;
            }
            else
            {
                generalControls.GetComponent<AdMobController>().ShowRewardedAd();
                generalControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
            }
            jokerQuantity.GetComponentInChildren<TMPro.TMP_Text>().text = veryEasyJokerQuantity.ToString();
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
}

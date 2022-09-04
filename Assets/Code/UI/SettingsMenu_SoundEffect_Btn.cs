using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_SoundEffect_Btn : MonoBehaviour
{
    public Sprite soundEffect_On;
    public Sprite soundEffect_Off;

    void Awake()
    {
        string soundEffect = PlayerPrefs.GetString("SoundEffect");
        if (soundEffect == "")
        {
            soundEffect = "1";
            PlayerPrefs.SetString("SoundEffect", soundEffect);
            PlayerPrefs.Save();
        }
        else if (int.Parse(soundEffect) == 1)
        {
            transform.GetComponent<Image>().sprite = soundEffect_On;
        }
        else if (int.Parse(soundEffect) == 0)
        {
            transform.GetComponent<Image>().sprite = soundEffect_Off;
        }
    }

    public void onClick()
    {
        int soundEffect = int.Parse(PlayerPrefs.GetString("SoundEffect"));
        if (soundEffect == 0)
        {
            transform.GetComponent<Image>().sprite = soundEffect_On;
            //müzikleri aç
            soundEffect = 1;
            GlobalVariable.soundEffectState = GlobalVariable.soundEffect_On;
            PlayerPrefs.SetString("SoundEffect", soundEffect.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            transform.GetComponent<Image>().sprite = soundEffect_Off;
            //müzikleri kapat
            soundEffect = 0;
            GlobalVariable.soundEffectState = GlobalVariable.soundEffect_Off;
            PlayerPrefs.SetString("SoundEffect", soundEffect.ToString());
            PlayerPrefs.Save();
        }
    }
}

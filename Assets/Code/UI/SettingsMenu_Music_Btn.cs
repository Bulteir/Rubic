using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_Music_Btn : MonoBehaviour
{
    public Sprite music_On;
    public Sprite music_Off;
    public AudioSource MenuMusic;
    public AudioSource normalModeMusic;
    public AudioSource challangeModeMusic;

    void Awake()
    {
        string music = PlayerPrefs.GetString("Music");
        if (music == "")
        {
            music = "1";
            PlayerPrefs.SetString("Music", music);
            PlayerPrefs.Save();
        }
        else if (int.Parse(music) == 1)
        {
            transform.GetComponent<Image>().sprite = music_On;
        }
        else if (int.Parse(music) == 0)
        {
            transform.GetComponent<Image>().sprite = music_Off;
        }
    }

    public void onClick()
    {
        int music = int.Parse(PlayerPrefs.GetString("Music"));
        if (music == 0)
        {
            transform.GetComponent<Image>().sprite = music_On;
            //müzikleri aç
            if (!MenuMusic.isPlaying)
                MenuMusic.Play();
            music = 1;
            GlobalVariable.musicState = GlobalVariable.musicState_On;
            PlayerPrefs.SetString("Music", music.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            transform.GetComponent<Image>().sprite = music_Off;
            //müzikleri kapat
            if (MenuMusic.isPlaying)
                MenuMusic.Pause();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Pause();
            if (challangeModeMusic.isPlaying)
                challangeModeMusic.Pause();
            music = 0;
            GlobalVariable.musicState = GlobalVariable.musicState_Off;
            PlayerPrefs.SetString("Music", music.ToString());
            PlayerPrefs.Save();
        }
    }
}

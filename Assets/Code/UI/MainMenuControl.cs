using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class MainMenuControl : MonoBehaviour
{
    public GameObject newGame_Btn;
    public GameObject mainMenuPanel;

    //Bu classý yeni oyun butonuna bastýktan sonra 2 yeni buton çýkma ihtimali için açtým. 
    //bir en iyi süreye karþý yarýþ bir normal mod.

    // Start is called before the first frame update
    void Start()
    {
        //newGame_Btn.GetComponentInChildren<TMP_Text>().text =  "deneme"; 
        //Debug.Log( LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Exit"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

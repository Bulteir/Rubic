using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuControl : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject bestTimePanel;
    public GameObject inGameUIPanel;
    public GameObject settingsPanel;

    public float spaceOfTop;
    public float spaceBetweenButtons;


    public List<GameObject> mainMenuButtons;

    void Start()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
        {
            inGameUIPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu && mainMenuPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_inGame && inGameUIPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
        }
    }

    //ekran döndüðünde çaðrýlýr
    public void uiScaleOptimizer()
    {
        float screenWidht = Screen.width;
        float screenHeight = Screen.height;

        //ana menü butonlarýnýn boyutlarýnýn ayarlanmasý

        foreach (var item in mainMenuButtons)
        {
            if (screenWidht > screenHeight)//yatay
            {
                float dynamicButtonWidth = (GlobalVariable.DDbuttonWidth / GlobalVariable.DDScreenWitdh) * screenWidht;
                float rate = GlobalVariable.DDbuttonWidth / GlobalVariable.DDbuttonHeight;

                //boyut ayarlamasý
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(dynamicButtonWidth, dynamicButtonWidth / rate);

                //font ayarlamasý
                item.GetComponentInChildren<TMP_Text>().enableAutoSizing = false;
                item.GetComponentInChildren<TMP_Text>().fontSize = (GlobalVariable.DDfontSize / GlobalVariable.DDbuttonWidth) * dynamicButtonWidth;
            }
            else// dikey
            {
                //referans telefonun redmi note 7'yi yatay konumdaki deðerlerini aldýðýmýz için dikey pozisyonda yatay/dikey için karþýt gloabal deðerleri kullanýyoruz
                float dynamicButtonheight = (GlobalVariable.DDbuttonHeight / GlobalVariable.DDScreenWitdh) * screenHeight;
                float rate = GlobalVariable.DDbuttonWidth / GlobalVariable.DDbuttonHeight;
                
                //boyut ayarlamasý
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(dynamicButtonheight * rate, dynamicButtonheight);
                
                //font ayarlamasý
                item.GetComponentInChildren<TMP_Text>().enableAutoSizing = false;
                item.GetComponentInChildren<TMP_Text>().fontSize = (GlobalVariable.DDfontSize / GlobalVariable.DDbuttonWidth) * (dynamicButtonheight * rate);
            }
        }


        float buttonHeight = mainMenuButtons[0].GetComponent<RectTransform>().rect.height;
        float totalButtonHeight = (mainMenuButtons.Count * buttonHeight) + ((mainMenuButtons.Count - 1) * spaceBetweenButtons);

        float firstButtonYPos = ((screenHeight - totalButtonHeight) / 2f) + spaceOfTop;
        float tempYPos = firstButtonYPos;

        foreach (var item in mainMenuButtons)
        {

            //ana menü butonlarnýn pozisyonun ayarlanmasý
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -tempYPos);
            tempYPos = tempYPos + spaceBetweenButtons + buttonHeight;
        }
    }
    
    //List içinde verilen textlerden en uzunu baz alarak hepsinin font size'nýn ne kadar olmasý gerektiði hesaplar.
    void calculateFontSize ()
    {
        int canditatendex = 0;
        float maxPreferedWitdh = 0;

        for (int i = 0; i < mainMenuButtons.Count; i++)
        {
            float preferredWidth = mainMenuButtons[i].GetComponentInChildren<TMP_Text>().preferredWidth;
            if(preferredWidth > maxPreferedWitdh)
            {
                maxPreferedWitdh = preferredWidth;
                canditatendex = i;
            }
        }


        mainMenuButtons[canditatendex].GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
        mainMenuButtons[canditatendex].GetComponentInChildren<TMP_Text>().ForceMeshUpdate();
        float optimumPointSize = mainMenuButtons[canditatendex].GetComponentInChildren<TMP_Text>().fontSize-5;
        mainMenuButtons[canditatendex].GetComponentInChildren<TMP_Text>().enableAutoSizing = false;


        foreach (var item in mainMenuButtons)
        {
            item.GetComponentInChildren<TMP_Text>().fontSize = optimumPointSize;
        }
    }

}

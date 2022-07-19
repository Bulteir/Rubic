using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_DeleteBestTimes_Btn : MonoBehaviour
{
    public GameObject popUp;
    public void onClick()
    {
        PlayerPrefs.DeleteKey("Bests");
        popUp.GetComponent<Lean.Gui.LeanWindow>().On = true;
    }
}

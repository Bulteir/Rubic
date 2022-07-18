using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_ShowRecords : MonoBehaviour
{
    public GameObject rubicCube;
    public void onClick()
    {
        //rubicCube.GetComponent<CubeControl>().getSortedBestTimeRecords();
        rubicCube.GetComponent<CubeControl>().showJsonBests();
    }
}

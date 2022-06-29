using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleCube_Btn : MonoBehaviour
{
    public Transform RubicCube;
    public GameObject counter;
    public void onClick()
    {
        Button shuffle_btn = gameObject.GetComponent<Button>();
        shuffle_btn.interactable = false;
        RubicCube.GetComponent<CubeControl>().shuffleCube(shuffle_btn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShuffleCube : MonoBehaviour
{
    public Transform RubicCube;

    public void onClick ()
    {
        RubicCube.GetComponent<CubeControl>().shuffleCube();
    }
}

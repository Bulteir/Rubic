using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetAvailabilityController : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 5.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {

            GlobalVariable.internetAvaible = false;
        }
        else
        {
            GlobalVariable.internetAvaible = true;
        }
    }
}

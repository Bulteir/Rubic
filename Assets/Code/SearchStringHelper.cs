using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStringHelper : MonoBehaviour
{
    public string SearchString;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.resolveHelper)
        {
            if(other.transform.name.Contains("Blue"))
                SearchString = "F";
            else if (other.transform.name.Contains("White"))
                SearchString = "U";
            else if (other.transform.name.Contains("Red"))
                SearchString = "R";
            else if (other.transform.name.Contains("Yellow"))
                SearchString = "L";
            else if (other.transform.name.Contains("Orange"))
                SearchString = "B";
            else if (other.transform.name.Contains("Green"))
                SearchString = "D";
        }
    }
}

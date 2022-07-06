using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFaceColorDetect : MonoBehaviour
{
    public int blue;
    public int red;
    public int green;
    public int yellow;
    public int white;
    public int orange;

    List<Transform> faceColors;


    // Start is called before the first frame update
    void Start()
    {
        faceColors = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.resolveHelper && !faceColors.Contains(other.transform))
        {
            faceColors.Add(other.transform);

            if (other.transform.name.Contains("Blue"))
                blue++;
            else if (other.transform.name.Contains("Red"))
                red++;
            else if (other.transform.name.Contains("Green"))
                green++;
            else if (other.transform.name.Contains("Yellow"))
                yellow++;
            else if (other.transform.name.Contains("White"))
                white++;
            else if (other.transform.name.Contains("Orange"))
                orange++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.resolveHelper)
        {
            faceColors.Remove(other.transform);
            if (other.transform.name.Contains("Blue"))
                blue--;
            else if (other.transform.name.Contains("Red"))
                red--;
            else if (other.transform.name.Contains("Green"))
                green--;
            else if (other.transform.name.Contains("Yellow"))
                yellow--;
            else if (other.transform.name.Contains("White"))
                white--;
            else if (other.transform.name.Contains("Orange"))
                orange--;
        }
    }

}

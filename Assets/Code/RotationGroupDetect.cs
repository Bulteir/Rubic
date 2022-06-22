using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGroupDetect : MonoBehaviour
{
    public List<GameObject> insideCubes;
    public Vector3 rotationAxis;

    private void Start()
    {
        insideCubes = new List<GameObject>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.rubicCube && !insideCubes.Contains(other.gameObject))
        {
            insideCubes.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.rubicCube)
        {
            insideCubes.Remove(other.gameObject);
        }
    }

}

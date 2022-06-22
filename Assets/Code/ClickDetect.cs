using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetect : MonoBehaviour
{
    public List<Transform> groupJoints; 

    // Start is called before the first frame update
    void Start()
    {
        groupJoints = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.groupJoint && !groupJoints.Contains(other.transform))
        {
            groupJoints.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GlobalVariable.groupJoint)
        {
            groupJoints.Remove(other.transform);
        }
    }
}

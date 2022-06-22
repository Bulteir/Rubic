using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
    public GameObject bottomGroupJoint;
    public GameObject horizontalMiddleGroupJoint;
    public GameObject topGroupJoint;
    public GameObject rightGroupJoint;
    public GameObject verticalMiddleGroupJoint;
    public GameObject leftGroupJoint;
    public GameObject frontGroupJoint;
    public GameObject frontMiddleGroupJoint;
    public GameObject backGroupJoint;

    float targetAngle = 90f;
    public float rotateSpeed = 1f;
    bool isRotateStarted = false;
    Transform selectedCubeGroupJoint;
    float rotateAngle;
    float acceleration = 1;
    Vector3 selectedAxis;
    Transform firstTouchedCube;
    Transform secondTouchedCube;
    public GameObject rubicCube;

    private void Update()
    {
        cubeSwipe();
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    void cubeSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isRotateStarted == false)
            {

                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariable.rubicCube)
                    {
                        firstTouchedCube = raycastHit.collider.transform;
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved && isRotateStarted == false && firstTouchedCube != null && secondTouchedCube == null)
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariable.rubicCube && raycastHit.collider.transform != firstTouchedCube)
                    {
                        secondTouchedCube = raycastHit.collider.transform;
                        List<Transform> firstCubeGroupJoints = firstTouchedCube.GetComponent<ClickDetect>().groupJoints;
                        List<Transform> secondCubeGroupJoints = secondTouchedCube.GetComponent<ClickDetect>().groupJoints;

                        foreach (var joint in firstCubeGroupJoints)
                        {

                            if (secondCubeGroupJoints.Contains(joint) && Vector3.Scale(joint.GetComponent<RotationGroupDetect>().rotationAxis, rubicCube.transform.InverseTransformVector(raycastHit.normal)) == Vector3.zero)
                            {
                                selectedCubeGroupJoint = joint;

                                List<GameObject> insideCubesFromGroup = selectedCubeGroupJoint.GetComponent<RotationGroupDetect>().insideCubes;

                                foreach (var cube in insideCubesFromGroup)
                                {
                                    cube.transform.parent = selectedCubeGroupJoint.transform.parent;
                                }
                                selectedAxis = joint.GetComponent<RotationGroupDetect>().rotationAxis;

                                Debug.DrawLine(firstTouchedCube.position, secondTouchedCube.position, Color.magenta, 5f, false);

                                float firstAngle = Vector3.SignedAngle(firstTouchedCube.localPosition, selectedCubeGroupJoint.position, rubicCube.transform.up);
                                float secondAngle = Vector3.SignedAngle(secondTouchedCube.localPosition, selectedCubeGroupJoint.position, rubicCube.transform.up);
                                Debug.Log("first:" + firstAngle + " second:" + secondAngle);


                                if (firstAngle < secondAngle)
                                {
                                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                }

                                if (firstAngle * secondAngle > 0 && firstAngle < secondAngle)
                                {
                                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                }
                                else if (firstAngle * secondAngle < 0 && Mathf.Abs(firstAngle) < Mathf.Abs(secondAngle))
                                {
                                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                }

                                Vector3 firstCubePixel = Camera.main.WorldToScreenPoint(firstTouchedCube.position);
                                Vector3 secondCubePixel = Camera.main.WorldToScreenPoint(secondTouchedCube.position);
                                float selectedCubeDeltaX = secondCubePixel.x - firstCubePixel.x;
                                float selectedCubeDeltaY = secondCubePixel.y - firstCubePixel.y;

                                #region Y ekseni (bottom, middle,top) için düzgün çalýþýyor
                                Debug.Log("yatay " + Mathf.Sign(rubicCube.transform.up.y));
                                if (firstCubePixel.x < secondCubePixel.x)
                                {
                                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                }

                                selectedAxis = new Vector3(0, selectedAxis.y * Mathf.Sign(rubicCube.transform.up.y), 0);
                                #endregion


                                startRotate();

                                break;
                            }
                        }

                    }
                }

            }

            if (touch.phase == TouchPhase.Ended)
            {
                firstTouchedCube = null;
                secondTouchedCube = null;
            }
        }
    }

    void Rotate()
    {
        if (isRotateStarted == true && selectedCubeGroupJoint != null)
        {
            if (rotateAngle < targetAngle)
            {
                selectedCubeGroupJoint.transform.parent.Rotate((rotateSpeed + acceleration) * selectedAxis.x, (rotateSpeed + acceleration) * selectedAxis.y, (rotateSpeed + acceleration) * selectedAxis.z, Space.Self);
                rotateAngle += rotateSpeed + acceleration;
                acceleration += 1;
            }
            else
            {
                selectedAxis = Vector3.zero;
                isRotateStarted = false;
                firstTouchedCube = null;
                secondTouchedCube = null;
            }
        }
    }
    void startRotate()
    {
        isRotateStarted = true;
        rotateAngle = 0;
        acceleration = 1;
    }
}

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
    public Transform rotationHelper;
    Vector3 startAngelsRotationHelper;


    private void Update()
    {
        cubeSwipe();
        rotateWithForce();

    }

    private void FixedUpdate()
    {
        //Rotate();
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
                        rotationHelper.GetComponent<Rigidbody>().isKinematic = false;
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
                                //string first = "";
                                //foreach (var item in firstCubeGroupJoints)
                                //{
                                //    first += item.name + ", ";
                                //}
                                //string second = "";
                                //foreach (var item in secondCubeGroupJoints)
                                //{
                                //    second += item.name + ", ";
                                //}
                                //Debug.Log("first:" + firstTouchedCube.name + " group:" + first + " second:" + secondTouchedCube.name + " group:" + second);
                                //Debug.Log("group:" + joint.name + " rotaionAxis:" + joint.GetComponent<RotationGroupDetect>().rotationAxis + " hit normal:" + raycastHit.normal);

                                selectedCubeGroupJoint = joint;

                                List<GameObject> insideCubesFromGroup = selectedCubeGroupJoint.GetComponent<RotationGroupDetect>().insideCubes;

                                foreach (var cube in insideCubesFromGroup)
                                {
                                    cube.transform.parent = selectedCubeGroupJoint.transform.parent;
                                }
                                selectedAxis = joint.GetComponent<RotationGroupDetect>().rotationAxis;

                                Debug.DrawLine(firstTouchedCube.position, secondTouchedCube.position, Color.magenta, 5f, false);

                                //float firstAngle = Vector3.SignedAngle(firstTouchedCube.localPosition, selectedCubeGroupJoint.position, rubicCube.transform.up);
                                //float secondAngle = Vector3.SignedAngle(secondTouchedCube.localPosition, selectedCubeGroupJoint.position, rubicCube.transform.up);
                                //Debug.Log("first:" + firstAngle + " second:" + secondAngle);


                                //if (firstAngle < secondAngle)
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //}

                                //if (firstAngle * secondAngle > 0 && firstAngle < secondAngle)
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //}
                                //else if (firstAngle*secondAngle <0 && Mathf.Abs(firstAngle) < Mathf.Abs(secondAngle))
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //}


                                //Vector3 temp = secondTouchedCube.localPosition - firstTouchedCube.localPosition;
                                //temp = new Vector3(temp.x * Mathf.Sign(rubicCube.transform.up.y), temp.y * Mathf.Sign(rubicCube.transform.up.y), temp.z * Mathf.Sign(rubicCube.transform.up.y));
                                //Debug.Log("direction:" + temp);

                                //selectedAxis = Vector3.zero;


                                //if (temp.x < 0)
                                //{
                                //    Debug.Log("sol" + temp.x);
                                //}
                                //else if (temp.x > 0)
                                //{
                                //    Debug.Log("sað" + temp.x);
                                //}

                                Vector3 firstCubePixel = Camera.main.WorldToScreenPoint(firstTouchedCube.position);
                                Vector3 secondCubePixel = Camera.main.WorldToScreenPoint(secondTouchedCube.position);
                                float selectedCubeDeltaX = secondCubePixel.x - firstCubePixel.x;
                                float selectedCubeDeltaY = secondCubePixel.y - firstCubePixel.y;

                                //Debug.Log("First cube:" + firstCubePixel + " Second cube:" + secondCubePixel + " up:" + rubicCube.transform.up);

                                //if (Mathf.Abs(selectedCubeDeltaX) > Mathf.Abs(selectedCubeDeltaY))//seçilen küpler yataysa
                                //{
                                //    Debug.Log("yatay " + Mathf.Sign(rubicCube.transform.up.y));
                                //    if (firstCubePixel.x < secondCubePixel.x)
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //    }

                                //}
                                //else//seçilen küpler dikeyse
                                //{
                                //    Debug.Log("dikey "+ Mathf.Sign(rubicCube.transform.up.y));

                                //    if (firstCubePixel.x < secondCubePixel.x)
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //    }

                                //}
                                #region Y ekseni (bottom, middle,top) için düzgün çalýþýyor
                                //Debug.Log("yatay " + Mathf.Sign(rubicCube.transform.up.y));
                                //if (firstCubePixel.x < secondCubePixel.x)
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(0, -1, 0));
                                //}

                                //selectedAxis = new Vector3(0, selectedAxis.y * Mathf.Sign(rubicCube.transform.up.y), 0);
                                #endregion


                                //Debug.Log("First cube:" + firstCubePixel + " Second cube:" + secondCubePixel + " right:" + rubicCube.transform.right + " up:" + rubicCube.transform.up + " forward:" + rubicCube.transform.forward);

                                //if (firstCubePixel.x < secondCubePixel.x)
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, 0, 0));
                                //}

                                //selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.right.x), 0, 0);

                                //if (Mathf.Abs(selectedCubeDeltaX) > Mathf.Abs(selectedCubeDeltaY))//seçilen küpler yataysa
                                //{
                                //    Debug.Log("yatay" + "x:" + Mathf.Sign(rubicCube.transform.up.x) + " y:" + Mathf.Sign(rubicCube.transform.up.y));

                                //    if (firstCubePixel.x < secondCubePixel.x)
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));
                                //    }
                                //    selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.right.x)* Mathf.Sign(rubicCube.transform.up.y), 0, 0);
                                //}
                                //else//seçilen küpler dikeyse
                                //{
                                //    Debug.Log("dikey" + "x:" + Mathf.Sign(rubicCube.transform.up.x) + " y:" + Mathf.Sign(rubicCube.transform.up.y));

                                //    if (firstCubePixel.y > secondCubePixel.y)
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));
                                //    }
                                //    selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.right.x) * Mathf.Sign(rubicCube.transform.up.x), 0, 0);
                                //}



                                //if (firstCubePixel.y > secondCubePixel.y)
                                //{
                                //    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));
                                //}
                                //selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.right.x) * Mathf.Sign(rubicCube.transform.up.x), 0, 0);

                                //selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.up.x) * Mathf.Sign(rubicCube.transform.up.y), 0, 0);

                                //Debug.Log("deltaX" + selectedCubeDeltaX + " deltaY" + selectedCubeDeltaY);

                                //if (selectedCubeDeltaX < 0 && selectedCubeDeltaY < 0)
                                //{
                                //    //    |
                                //    // ---|---
                                //    //  X |
                                //    Debug.Log("sol alt");

                                //    if (Mathf.Abs(selectedCubeDeltaX) < Mathf.Abs(selectedCubeDeltaY))//dikey
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                                //    }
                                //    else // yatay
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));
                                //    }

                                //}
                                //else if (selectedCubeDeltaX < 0 && selectedCubeDeltaY > 0)
                                //{
                                //    //  X |
                                //    // ---|---
                                //    //    |
                                //    Debug.Log("sol üst");
                                //    if (Mathf.Abs(selectedCubeDeltaX) < Mathf.Abs(selectedCubeDeltaY))//dikey
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                                //    }
                                //    else // yatay
                                //    {
                                //        //burda bir sýkýntý oluþuyor ek bir durum daha var
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                                //    }

                                //}
                                //else if (selectedCubeDeltaX > 0 && selectedCubeDeltaY > 0)
                                //{
                                //    //    | X
                                //    // ---|---
                                //    //    |
                                //    Debug.Log("sað üst");
                                //    if (Mathf.Abs(selectedCubeDeltaX) < Mathf.Abs(selectedCubeDeltaY))//dikey
                                //    {
                                //        //burda bir sýkýntý oluþuyor ek bir durum daha var

                                //        //selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));
                                //    }
                                //    else // yatay
                                //    {

                                //    }
                                //}
                                //else if (selectedCubeDeltaX > 0 && selectedCubeDeltaY < 0)
                                //{
                                //    //    | 
                                //    // ---|---
                                //    //    | X

                                //    Debug.Log("sað alt");
                                //    if (Mathf.Abs(selectedCubeDeltaX) < Mathf.Abs(selectedCubeDeltaY))//dikey
                                //    {

                                //    }
                                //    else // yatay
                                //    {
                                //        selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                                //    }
                                //}
                                //else
                                //{
                                //    Debug.Log("buraya nasýl girer");

                                //}

                                //selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(rubicCube.transform.right.x), 0, 0);

                                rotationHelper.position = selectedCubeGroupJoint.parent.position;
                                rotationHelper.rotation = selectedCubeGroupJoint.parent.rotation;
                                rotationHelper.GetComponent<BoxCollider>().size = selectedCubeGroupJoint.GetComponent<BoxCollider>().size;
                                startAngelsRotationHelper = rotationHelper.eulerAngles;
                                rotationHelper.GetComponent<Rigidbody>().AddForceAtPosition(secondTouchedCube.position - firstTouchedCube.position, firstTouchedCube.position, ForceMode.VelocityChange);

                                break;
                            }
                        }

                    }
                }

            }

            if (touch.phase == TouchPhase.Ended)
            {

                #region bir rotation helper kullanrak hesaplamaya çalýþtýk ancak x ekseni sorunundan olmadý.
                //Debug.Log("selected group x:" + selectedCubeGroupJoint.parent.eulerAngles.x + " start Rot helper:" + startAngelsRotationHelper.x + " rot helper: "+rotationHelper.eulerAngles.x+" startAngle - rotAngle:" + (startAngelsRotationHelper.x - rotationHelper.eulerAngles.x));
                //if (rotationHelper.eulerAngles.x  >  selectedCubeGroupJoint.eulerAngles.x)
                //{
                //    selectedAxis = new Vector3(selectedAxis.x, 0, 0);

                //}
                //else if (rotationHelper.eulerAngles.x < selectedCubeGroupJoint.eulerAngles.x)
                //{
                //    selectedAxis = new Vector3(selectedAxis.x*-1, 0, 0);
                //}
                #endregion
                //selectedAxis = new Vector3(selectedAxis.x, 0, 0);

                //startRotate();


                firstTouchedCube = null;
                secondTouchedCube = null;
                //rotationHelper.GetComponent<Rigidbody>().isKinematic = true;
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

    void rotateWithForce()
    {
        if (rotationHelper != null && selectedCubeGroupJoint != null)
        {
            float tempXangle = rotationHelper.eulerAngles.x;
            if (rotationHelper.rotation.eulerAngles.y == 180 && rotationHelper.rotation.eulerAngles.z == 180)
                tempXangle = 180 - tempXangle;

            selectedCubeGroupJoint.parent.transform.eulerAngles = new Vector3(tempXangle, 0, 0); 
        }
    }
}

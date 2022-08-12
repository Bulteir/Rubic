using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCameraControl : MonoBehaviour
{
    public Transform cube;
    public float cubeRotationSpeed = 200;
    public float cubeRotationSpeedMobile = 20;
    public float lerpVal = 5;
    public float lerpValMobile = 5;

    float xRotation;
    float yRotation;

    float lastXRotationVal;
    float lastYRotationVal;

    bool dragRubicCube = false;
    bool toucedRubicCube = false;
    public bool isEnteredPauseButton = false;
    public bool isEnteredSolveButton = false;
    GameObject[] faces;
    Vector3 tiltAxis;
    public bool isTiltRubicCube = false;

    void Start()
    {
        faces = GameObject.FindGameObjectsWithTag(GlobalVariable.FaceDetect);
        tiltAxis = Vector3.zero;
    }

    private void Update()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
            controlWithTouch();
    }

    void controlWithTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)//burada küpü tek parmakla döndüreceðiz
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform.gameObject.tag != GlobalVariable.rubicCube && raycastHit.transform.gameObject.tag != GlobalVariable.groupJoint && raycastHit.transform.gameObject.tag != GlobalVariable.touchHelper)
                        {
                            dragRubicCube = true;
                            toucedRubicCube = false;
                            isEnteredPauseButton = false;
                            isEnteredSolveButton = false;
                        }
                        else
                        {
                            lastXRotationVal = 0;
                            lastYRotationVal = 0;
                            dragRubicCube = false;
                            toucedRubicCube = true;
                        }
                    }
                    else
                    {
                        isEnteredPauseButton = false;
                        isEnteredSolveButton = false;

                        dragRubicCube = true;
                        toucedRubicCube = false;
                    }
                }

                if (touch.phase == TouchPhase.Moved && dragRubicCube == true)
                {
                    xRotation = touch.deltaPosition.x * cubeRotationSpeedMobile * Mathf.Deg2Rad;
                    yRotation = touch.deltaPosition.y * cubeRotationSpeedMobile * Mathf.Deg2Rad;

                    cube.Rotate(Camera.main.transform.up, -xRotation, Space.World);
                    cube.Rotate(Camera.main.transform.right, yRotation, Space.World);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    lastXRotationVal = xRotation;
                    lastYRotationVal = yRotation;
                    dragRubicCube = false;
                }
            }
            else if (Input.touchCount == 2)// iki parmak varsa ekranda tilt hareketi yaptýracaðýz
            {
                Touch finger1 = Input.GetTouch(0);
                Touch finger2 = Input.GetTouch(1);

                if (finger1.phase == TouchPhase.Began || finger2.phase == TouchPhase.Began)
                {
                    foreach (var item in faces)
                    {
                        item.layer = 0;//default
                    }

                    //burada küpün hangi yüzü kameraya en yakýn bulacaðýz ve ona göre tilt hareketinin hangi axiste yapýlacaðýný set edeceðiz.
                    Vector2 delta = (finger1.position + finger2.position) / 2f;
                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(delta);

                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform.gameObject.name.Contains("front"))
                        {
                            tiltAxis = new Vector3(0, 0, 1);
                            isTiltRubicCube = true;
                        }
                        else if (raycastHit.transform.gameObject.name.Contains("back"))
                        {
                            tiltAxis = new Vector3(0, 0, -1);
                            isTiltRubicCube = true;
                        }
                        else if (raycastHit.transform.gameObject.name.Contains("top"))
                        {
                            tiltAxis = new Vector3(0, -1, 0);
                            isTiltRubicCube = true;
                        }
                        else if (raycastHit.transform.gameObject.name.Contains("down"))
                        {
                            tiltAxis = new Vector3(0, 1, 0);
                            isTiltRubicCube = true;
                        }
                        else if (raycastHit.transform.gameObject.name.Contains("right"))
                        {
                            tiltAxis = new Vector3(-1, 0, 0);
                            isTiltRubicCube = true;
                        }
                        else if (raycastHit.transform.gameObject.name.Contains("left"))
                        {
                            tiltAxis = new Vector3(1, 0, 0);
                            isTiltRubicCube = true;
                        }
                    }
                }

                if ((finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved) && isTiltRubicCube)
                {
                    Vector2 prevFinger1Pos = finger1.position - finger1.deltaPosition;
                    Vector2 prevFinger2Pos = finger2.position - finger2.deltaPosition;

                    Vector2 prevDir = prevFinger2Pos - prevFinger1Pos;
                    Vector2 currDir = finger2.position - finger1.position;
                    float angle = Vector2.SignedAngle(prevDir, currDir);
                    cube.Rotate(tiltAxis*angle);
                }

                if (finger1.phase == TouchPhase.Ended || finger2.phase == TouchPhase.Ended)
                {
                    foreach (var item in faces)
                    {
                        item.layer = 2;//ignore raycast
                    }
                    tiltAxis = Vector3.zero;
                }
            }
        }
        else//ekrana dokunan hiç bir parmak yoksa 
        {
            isTiltRubicCube = false;//tilt bitmiþtir diðer hareketler yapýlabilir. Özellikle küp renkerli çevirme
        }

        if (dragRubicCube == false && toucedRubicCube == false && isEnteredPauseButton == false && isEnteredSolveButton == false)
        {
            if (Mathf.Abs(lastXRotationVal) > 0.01f)
            {
                lastXRotationVal = Mathf.Lerp(lastXRotationVal, 0f, lerpValMobile * Time.deltaTime);
                cube.Rotate(Camera.main.transform.up, -lastXRotationVal, Space.World);
            }

            if (Mathf.Abs(lastYRotationVal) > 0.01f)
            {
                lastYRotationVal = Mathf.Lerp(lastYRotationVal, 0f, lerpValMobile * Time.deltaTime);
                cube.Rotate(Camera.main.transform.right, lastYRotationVal, Space.World);
            }
        }
    }
}

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
    
    private void Update()
    {

        if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
            controlWithTouch();
    }

    void controlWithTouch()
    {
        if (Input.touchCount > 0)
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

        if (dragRubicCube == false && toucedRubicCube == false && isEnteredPauseButton == false)
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

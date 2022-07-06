using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    public int shuffleStepCount;//toplam kaç kez karýþtýracak
    int shuffleStep = 0;//þuan karýþtýrmada kaçýncý adýmda
    
    public GameObject counter;

    public GameObject rubicCubePrefab;
    List<Transform> rubicCubeItems;
    List<Transform> rubicCubePrefabItems;

    public List<GameObject> faceColorDetectors;

    void Start()
    {
        rubicCubeItems = new List<Transform>();
        rubicCubePrefabItems = new List<Transform>();

        //rubik küpün tüm elemanlarýný bir listeye ekler
        rubicToList(transform.parent, rubicCubeItems);
        //rubik küp prefabýnýn tüm elemanlarýný bir listeye ekler
        rubicToList(rubicCubePrefab.transform, rubicCubePrefabItems);
    }

    private void Update()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
            cubeSwipe();
    }

    private void FixedUpdate()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
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
                    if ((raycastHit.collider.gameObject.tag == GlobalVariable.rubicCube || raycastHit.collider.gameObject.tag == GlobalVariable.touchHelper) && raycastHit.collider.transform != firstTouchedCube)
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

                                #region vector3.cross 2 vetörün çapraz çarpýmý. Yani iki vectöre dik olan 3. bir vectör döner. Bu sayede dönme yönünü bulabiliyoruz.

                                //iki vectöre inen dikmeyi bulunca selectedcube'in ekseni ile karþýlaþtýrýp çarpýyoruz. Bu sayede zýt yönlüler ise tersi yönde dönüþ saðlanýyor.
                                Vector3 side1 = firstTouchedCube.position - selectedCubeGroupJoint.position;
                                Vector3 side2 = secondTouchedCube.position - selectedCubeGroupJoint.position;

                                Vector3 cross = Vector3.Cross(side1, side2).normalized;
                                #endregion

                                selectedAxis = new Vector3(selectedAxis.x * Mathf.Sign(cross.x) * Mathf.Sign(selectedCubeGroupJoint.right.x), selectedAxis.y * Mathf.Sign(cross.y) * Mathf.Sign(selectedCubeGroupJoint.up.y), selectedAxis.z * Mathf.Sign(cross.z) * Mathf.Sign(selectedCubeGroupJoint.forward.z));

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
                float rotationValue = rotateSpeed + acceleration;
                if (targetAngle - rotateAngle < acceleration + rotateSpeed)
                    rotationValue = targetAngle - rotateAngle;

                selectedCubeGroupJoint.transform.parent.Rotate(rotationValue * selectedAxis.x, rotationValue * selectedAxis.y, rotationValue * selectedAxis.z, Space.Self);
                rotateAngle += rotationValue;
                acceleration += rotateSpeed;
            }
            else
            {
                selectedAxis = Vector3.zero;
                isRotateStarted = false;
                firstTouchedCube = null;
                secondTouchedCube = null;
                isResolveCube();
            }
        }
    }
    void startRotate()
    {
        isRotateStarted = true;
        rotateAngle = 0;
        acceleration = 1;
    }

    public void shuffleCube(Button button)
    {
        StartCoroutine(shuffleCoroutine(button));
    }
    IEnumerator shuffleCoroutine(Button button)
    {
        float firstRotationSpeed = rotateSpeed;
        rotateSpeed = firstRotationSpeed + 5;
        while (shuffleStep < shuffleStepCount)
        {
            if (isRotateStarted == false)
            {
                int randomGroupJoint = Random.Range(0, 9);

                List<GameObject> joints = new List<GameObject>{bottomGroupJoint,
                                                        horizontalMiddleGroupJoint,
                                                        topGroupJoint,
                                                        rightGroupJoint,
                                                        verticalMiddleGroupJoint,
                                                        leftGroupJoint,
                                                        frontGroupJoint,
                                                        frontMiddleGroupJoint,
                                                        backGroupJoint};

                Transform randomSelectedGroupJoint = joints[randomGroupJoint].transform;

                selectedCubeGroupJoint = randomSelectedGroupJoint;

                List<GameObject> insideCubesFromGroup = selectedCubeGroupJoint.GetComponent<RotationGroupDetect>().insideCubes;

                foreach (var cube in insideCubesFromGroup)
                {
                    cube.transform.parent = selectedCubeGroupJoint.transform.parent;
                }
                selectedAxis = randomSelectedGroupJoint.GetComponent<RotationGroupDetect>().rotationAxis;

                int randomDirection = Random.Range(0, 2);

                if (randomDirection == 0)
                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                shuffleStep++;
                startRotate();
                yield return new WaitUntil(() => isRotateStarted == false);
            }
        }
        shuffleStep = 0;
        rotateSpeed = firstRotationSpeed;
        button.interactable = true;
        counter.GetComponent<Counter>().resetCounter();
        counter.GetComponent<Counter>().startCounter();
    }

    public void resetRubicCube()
    {
        foreach (var item in rubicCubeItems)
        {
            //herbir rubik küp elementinin position ve rotation'ýnýný en baþtaki haline getiriyoruz 
            Transform itemPrefab = rubicCubePrefabItems.Find(x => x.name == item.name);
            if (itemPrefab != null)
            {
                item.position = itemPrefab.position;
                item.rotation = itemPrefab.rotation;
            }

            //herbir rubik küp elementinin parentýný en baþtaki haline getiriyoruz 
            if (itemPrefab.parent != null)
            {
                Transform parentItem = rubicCubeItems.Find(x => x.name == itemPrefab.parent.name);
                if (parentItem != null)
                {
                    item.parent = parentItem;
                }
            }
        }
    }

    public void resetRubicCube(Button button)
    {
        foreach (var item in rubicCubeItems)
        {
            //herbir rubik küp elementinin position ve rotation'ýnýný en baþtaki haline getiriyoruz 
            Transform itemPrefab = rubicCubePrefabItems.Find(x => x.name == item.name);
            if (itemPrefab != null)
            {
                item.position = itemPrefab.position;
                item.rotation = itemPrefab.rotation;
            }

            //herbir rubik küp elementinin parentýný en baþtaki haline getiriyoruz 
            if (itemPrefab.parent != null)
            {
                Transform parentItem = rubicCubeItems.Find(x => x.name == itemPrefab.parent.name);
                if (parentItem != null)
                {
                    item.parent = parentItem;
                }
            }
        }
        StartCoroutine(restartSuffleHelper(button));
    }

    IEnumerator restartSuffleHelper(Button button)
    {
        yield return new WaitForFixedUpdate();
        shuffleCube(button);
    }

    void rubicToList(Transform cube, List<Transform> itemsList)
    {
        itemsList.Add(cube);
        int childCount = cube.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cube.GetChild(i);
            rubicToList(child, itemsList);
        }
    }

    void isResolveCube()
    {
        int resolvedFace = 0;
        foreach (var detector in faceColorDetectors)
        {
            if (detector.GetComponent<CubeFaceColorDetect>().blue == 9)
                resolvedFace++;
            else if (detector.GetComponent<CubeFaceColorDetect>().red == 9)
                resolvedFace++;
            else if (detector.GetComponent<CubeFaceColorDetect>().green == 9)
                resolvedFace++;
            else if (detector.GetComponent<CubeFaceColorDetect>().yellow == 9)
                resolvedFace++;
            else if (detector.GetComponent<CubeFaceColorDetect>().white == 9)
                resolvedFace++;
            else if (detector.GetComponent<CubeFaceColorDetect>().orange == 9)
                resolvedFace++;
        }

        if (resolvedFace == 6)
        {
            Debug.Log("Tebrikler Kazandýnýz");
        }
    }




}

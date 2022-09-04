using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCubeAnimation : MonoBehaviour
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
    public float rotateSpeed = 0.5f;
    bool isRotateStarted = false;

    Transform selectedCubeGroupJoint;

    Vector3 selectedAxis;
    float rotateAngle;
    float acceleration = 1;

    public GameObject rubicCubePrefab;
    List<Transform> rubicCubeItems;
    List<Transform> rubicCubePrefabItems;

    // Start is called before the first frame update
    void Start()
    {
        rubicCubeItems = new List<Transform>();
        rubicCubePrefabItems = new List<Transform>();

        //rubik küpün tüm elemanlarýný bir listeye ekler
        rubicToList(transform.parent, rubicCubeItems);
        //rubik küp prefabýnýn tüm elemanlarýný bir listeye ekler
        rubicToList(rubicCubePrefab.transform, rubicCubePrefabItems);
        shuffleCube();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu)
        {
            Rotate();
            transform.parent.RotateAround(transform.parent.position, new Vector3(0, 1, 0), 1f);
        }
    }

    public void shuffleCube()
    {
        StartCoroutine(shuffleCoroutineMenuCube());
    }

    IEnumerator shuffleCoroutineMenuCube()
    {
        float firstRotationSpeed = rotateSpeed;

        while (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu)
        {
            if (isRotateStarted == false)
            {
                int randomGroupJoint = UnityEngine.Random.Range(0, 9);

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

                int randomDirection = UnityEngine.Random.Range(0, 2);

                if (randomDirection == 0)
                    selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

                startRotate();
                yield return new WaitUntil(() => isRotateStarted == false);
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
                selectedCubeGroupJoint = null;
                isRotateStarted = false;
            }
        }
    }
    void startRotate()
    {
        isRotateStarted = true;
        rotateAngle = 0;
        acceleration = 1;
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

    private void OnDisable()
    {
        StopCoroutine(shuffleCoroutineMenuCube());
    }
}

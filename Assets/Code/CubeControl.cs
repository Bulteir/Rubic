using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

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
    public bool isRotateStarted = false;
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

    public float victoryExplodeForce;
    public float victoryExplodeRange;
    public TMP_Text victoryMessage;
    public int resolveMoves;
    public TMP_Text Moves;

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
        if (GlobalVariable.gameState == GlobalVariable.gameState_inGame && shuffleStep == 0)
        {
            resolveMoves++;
            Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
        }
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

            if (item.GetComponent<Rigidbody>() && item.GetComponent<BoxCollider>() && item.tag == GlobalVariable.rubicCube)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<BoxCollider>().isTrigger = true;
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
            VictoryCelebration();
        }
    }

    void VictoryCelebration()
    {
        List<Transform> tempItems = rubicCubeItems.FindAll(x => x.tag == GlobalVariable.rubicCube && x.GetComponent<ClickDetect>());

        foreach (var item in tempItems)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.GetComponent<Rigidbody>().useGravity = true;
            item.GetComponent<Rigidbody>().AddExplosionForce(victoryExplodeForce, rubicCube.transform.position, victoryExplodeRange);
            item.GetComponent<BoxCollider>().isTrigger = false;
        }

        victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
            LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + counter.GetComponent<TMP_Text>().text + " " +
            LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
        GlobalVariable.gameState = GlobalVariable.gameState_Victory;

        SaveBestTime(counter.GetComponent<TMP_Text>().text, resolveMoves);
    }

    [System.Serializable]
    public struct BestTimesStruct
    {
        public string time;
        public int moves;
        public string RecordTime;
    }

    [System.Serializable]
    public class JsonableListWrapper<T>
    {
        public List<T> list;
        public JsonableListWrapper(List<T> list) => this.list = list;
    }

    void SaveBestTime(string time, int moves)
    {

        string json = PlayerPrefs.GetString("Bests");
        List<BestTimesStruct> bestTimesList = new List<BestTimesStruct>();
        if (json != "")
        {
            bestTimesList = JsonUtility.FromJson<JsonableListWrapper<BestTimesStruct>>(json).list;

            BestTimesStruct newBestTime = new BestTimesStruct();
            newBestTime.moves = moves;
            newBestTime.time = time;
            newBestTime.RecordTime = System.DateTime.Today.ToShortDateString();
            bestTimesList.Add(newBestTime);

            bestTimesList.Sort((x, y) => x.time.CompareTo(y.time));

            if (bestTimesList.Count > 5)
            {
                if (bestTimesList[5].time != time)
                {
                    victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + counter.GetComponent<TMP_Text>().text + " " +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
                }
                bestTimesList.RemoveAt(5);
            }
            else
            {
                victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + counter.GetComponent<TMP_Text>().text + " " +
                       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
            }
        }
        else
        {
            BestTimesStruct newBestTime = new BestTimesStruct();
            newBestTime.moves = moves;
            newBestTime.time = time;
            newBestTime.RecordTime = System.DateTime.Today.ToShortDateString();
            bestTimesList.Add(newBestTime);

            victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + counter.GetComponent<TMP_Text>().text + " " +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
        }

        string tempJson = JsonUtility.ToJson(new JsonableListWrapper<BestTimesStruct>(bestTimesList));

        PlayerPrefs.SetString("Bests", tempJson);
        PlayerPrefs.Save();
    }

    public void getBestTimes(TMP_Text bestTimeLabel)
    {
        string json = PlayerPrefs.GetString("Bests");

        string bestTimesString = "       " + LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time") + "           " +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Move") + "	    " +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Date") + "\n";

        if (json != "")
        {
            List<BestTimesStruct> bestTimes = JsonUtility.FromJson<JsonableListWrapper<BestTimesStruct>>(json).list;

            for (int i = 0; i < bestTimes.Count; i++)
            {
                bestTimesString += (i + 1) + "- " + bestTimes[i].time + "\t\t" + bestTimes[i].moves + "\t" + bestTimes[i].RecordTime + "\n";
            }

            for (int i = bestTimes.Count; i < 5; i++)
            {
                bestTimesString += (i + 1) + "-" + "\t-" + "\t\t-\t        -\n";
            }

            bestTimeLabel.text = bestTimesString;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                bestTimesString += (i + 1) + "-" + "\t-" + "\t\t-\t        -\n";
            }

            bestTimeLabel.text = bestTimesString;
        }
    }

    public void rotateForKociemba(string solution)
    {
        StartCoroutine(rotateHelperForKociemba(solution));
    }

    IEnumerator rotateHelperForKociemba(string solution)
    {
        if (isRotateStarted == false)
        {
            string[] resolveSteps = solution.Split(" ");

            foreach (var item in resolveSteps)
            {
                bool clockwise = true;
                if (item != "")
                {
                    if (item.Contains("F"))
                        selectedCubeGroupJoint = frontGroupJoint.transform;
                    else if (item.Contains("B"))
                        selectedCubeGroupJoint = backGroupJoint.transform;
                    else if (item.Contains("R"))
                        selectedCubeGroupJoint = rightGroupJoint.transform;
                    else if (item.Contains("L"))
                        selectedCubeGroupJoint = leftGroupJoint.transform;
                    else if (item.Contains("U"))
                        selectedCubeGroupJoint = topGroupJoint.transform;
                    else if (item.Contains("D"))
                        selectedCubeGroupJoint = bottomGroupJoint.transform;

                    if (item.Contains("\'"))
                        clockwise = false;
                    else if (item.Contains("2"))
                    {
                        rotateHelpersHelperForKociemba(clockwise);
                        yield return new WaitUntil(() => isRotateStarted == false);
                    }

                    rotateHelpersHelperForKociemba(clockwise);
                    yield return new WaitUntil(() => isRotateStarted == false);
                }
            }
        }
    }

    void rotateHelpersHelperForKociemba(bool rotateClockWise)
    {
        List<GameObject> insideCubesFromGroup = selectedCubeGroupJoint.GetComponent<RotationGroupDetect>().insideCubes;

        foreach (var cube in insideCubesFromGroup)
        {
            cube.transform.parent = selectedCubeGroupJoint.transform.parent;
        }
        selectedAxis = selectedCubeGroupJoint.GetComponent<RotationGroupDetect>().rotationAxisForKociembaResolver;

        if (rotateClockWise == false)
            selectedAxis = Vector3.Scale(selectedAxis, new Vector3(-1, -1, -1));

        startRotate();
    }

    public void rotateAndFixCubeForKociembaStart(List<Transform> faceDetectors, Transform solve_Btn)
    {
        StartCoroutine(rotateAndFixCubeForKociemba(faceDetectors, solve_Btn));
    }

    IEnumerator rotateAndFixCubeForKociemba(List<Transform> faceDetectors, Transform solve_Btn)
    {
        #region doðru çözüm için gerekli olan front face orta küpü düzeltiyoruz.
        while (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString != "F")
        {
            if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "L")//frontFace orta küp detektörü
            {
                selectedCubeGroupJoint = horizontalMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(true);
            }
            else if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "R")
            {
                selectedCubeGroupJoint = horizontalMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(false);
            }
            else if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "U")
            {
                selectedCubeGroupJoint = verticalMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(true);
            }
            else if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "D")
            {
                selectedCubeGroupJoint = verticalMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(false);
            }
            else if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "B")
            {
                selectedCubeGroupJoint = horizontalMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(true);
                yield return new WaitUntil(() => isRotateStarted == false);
                rotateHelpersHelperForKociemba(true);
            }
            yield return new WaitUntil(() => isRotateStarted == false);
        }
        #endregion

        #region doðru çözüm için gerekli olan front face orta küpü düzeltiyoruz.
        while (faceDetectors[0].GetChild(4).GetComponent<SearchStringHelper>().SearchString != "U")
        {
            if (faceDetectors[0].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "R")
            {
                selectedCubeGroupJoint = frontMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(true);
            }
            else if (faceDetectors[0].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "L")
            {
                selectedCubeGroupJoint = frontMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(false);
            }
            else if (faceDetectors[0].GetChild(4).GetComponent<SearchStringHelper>().SearchString == "D")
            {
                selectedCubeGroupJoint = frontMiddleGroupJoint.transform;
                rotateHelpersHelperForKociemba(true);
                yield return new WaitUntil(() => isRotateStarted == false);
                rotateHelpersHelperForKociemba(true);
            }
            yield return new WaitUntil(() => isRotateStarted == false);
        }

        string solution = solve_Btn.GetComponent<Solve_Btn>().getSolution();

        #endregion
        rotateForKociemba(solution);
    }

}

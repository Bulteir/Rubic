using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using System;

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

    public bool isShuffleRotation = false;
    public Button solve_Btn;
    public GameObject GeneralControls;

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

                                //oluþturulan çözüm stringi sonrasýnda elle müdahalle olduðunda yeniden çözüm stringi hesaplatmak için
                                PlayerPrefs.DeleteKey("Solution");

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
                if (isShuffleRotation == false)
                    isResolveCube();
                if (solve_Btn.GetComponent<Solve_Btn>().isSolvingStepActive && solve_Btn.GetComponent<Solve_Btn>().isDoubleSolvingStep == false)
                {
                    solve_Btn.interactable = true;
                    solve_Btn.GetComponent<Solve_Btn>().isSolvingStepActive = false;
                    solve_Btn.GetComponent<Solve_Btn>().SetSolvingQuantityBtnInteractable(true);
                }
                else
                {
                    solve_Btn.GetComponent<Solve_Btn>().isDoubleSolvingStep = false;
                }
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
        isShuffleRotation = true;
   
        float firstRotationSpeed = rotateSpeed;
        if (button != solve_Btn)
            rotateSpeed = firstRotationSpeed + 5;

        if (shuffleStepCount > 1 && GeneralControls.GetComponent<MenuControl>().isLoadedKociembaTables)//yeni oyun ve restarttan gelen shuffle'dýr
        {
            solve_Btn.interactable = false;
            solve_Btn.GetComponent<Solve_Btn>().SetSolvingQuantityBtnInteractable(false);
        }

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
        if (button != solve_Btn)
        {
            button.interactable = true;
            counter.GetComponent<Counter>().resetCounter();
            counter.GetComponent<Counter>().startCounter();
        }

        if (shuffleStepCount > 1 && GeneralControls.GetComponent<MenuControl>().isLoadedKociembaTables)//yeni oyun ve restarttan gelen shuffle'dýr
        {
            solve_Btn.interactable = true;
            solve_Btn.GetComponent<Solve_Btn>().SetSolvingQuantityBtnInteractable(true);
        }
        isShuffleRotation = false;
    }

    public void resetRubicCube()
    {
        StopAllCoroutines();
        isRotateStarted = false;
        shuffleStep = 0;
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

        if (GeneralControls.GetComponent<MenuControl>().isLoadedKociembaTables == true)
        {
            solve_Btn.interactable = true;
            solve_Btn.GetComponent<Solve_Btn>().SetSolvingQuantityBtnInteractable(true);
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
            GeneralControls.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();
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

        if(counter.GetComponent<Counter>().isChallengeModeActive)
        {
            string json = PlayerPrefs.GetString("Bests");
            List<CubeControl.BestTimesStruct> bestTimesList = new List<CubeControl.BestTimesStruct>();
            bestTimesList = JsonUtility.FromJson<CubeControl.JsonableListWrapper<CubeControl.BestTimesStruct>>(json).list;

            string bestTime = counter.GetComponent<Counter>().GetDifferenceTwoTimes(bestTimesList[0].time, counter.GetComponent<TMP_Text>().text);


            SaveBestTime(bestTime, resolveMoves);
            GlobalVariable.gameState = GlobalVariable.gameState_Victory;
        }
        else
        {
            victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + counter.GetComponent<TMP_Text>().text + " " +
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;

            SaveBestTime(counter.GetComponent<TMP_Text>().text, resolveMoves);
            GlobalVariable.gameState = GlobalVariable.gameState_Victory;
        }
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
            newBestTime.time = time.Replace("\n", "");
            newBestTime.RecordTime = System.DateTime.Today.ToShortDateString();
            bestTimesList.Add(newBestTime);

            bestTimesList.Sort((x, y) => x.time.CompareTo(y.time));

            if (bestTimesList.Count > 5)
            {
                if (bestTimesList[5].time != time)
                {
                    victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
                        LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
                }
                bestTimesList.RemoveAt(5);
            }
            else
            {
                victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
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
                LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
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
                bestTimesString += (i + 1) + "- " + bestTimes[i].time.Replace("\n", "") + "\t\t" + bestTimes[i].moves + "\t" + bestTimes[i].RecordTime + "\n";
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
        StartCoroutine(rotateHelperForKociembaForEveryTouch(solution));
    }

    IEnumerator rotateHelperForKociembaForEveryTouch(string solution)
    {
        if (isRotateStarted == false)
        {
            string[] resolveSteps = solution.Split(" ");
            bool clockwise = true;
            if (resolveSteps.Length > 0)
            {
                if (resolveSteps[0] != "")
                {
                    if (resolveSteps[0].Contains("F"))
                        selectedCubeGroupJoint = frontGroupJoint.transform;
                    else if (resolveSteps[0].Contains("B"))
                        selectedCubeGroupJoint = backGroupJoint.transform;
                    else if (resolveSteps[0].Contains("R"))
                        selectedCubeGroupJoint = rightGroupJoint.transform;
                    else if (resolveSteps[0].Contains("L"))
                        selectedCubeGroupJoint = leftGroupJoint.transform;
                    else if (resolveSteps[0].Contains("U"))
                        selectedCubeGroupJoint = topGroupJoint.transform;
                    else if (resolveSteps[0].Contains("D"))
                        selectedCubeGroupJoint = bottomGroupJoint.transform;

                    if (resolveSteps[0].Contains("\'"))
                        clockwise = false;
                    else if (resolveSteps[0].Contains("2"))
                    {
                        rotateHelpersHelperForKociemba(clockwise);
                        solve_Btn.GetComponent<Solve_Btn>().isDoubleSolvingStep = true;
                        yield return new WaitUntil(() => isRotateStarted == false);
                    }

                    rotateHelpersHelperForKociemba(clockwise);
                    yield return new WaitUntil(() => isRotateStarted == false);

                    string solutionString = "";
                    for (int i = 1; i < resolveSteps.Length; i++)
                    {
                        solutionString += resolveSteps[i] + " ";
                    }

                    PlayerPrefs.SetString("Solution", solutionString);
                    PlayerPrefs.Save();
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
        StartCoroutine(rotateAndFixCubeForKociembaForEveryTouch(faceDetectors, solve_Btn));
    }

    IEnumerator rotateAndFixCubeForKociembaForEveryTouch(List<Transform> faceDetectors, Transform solveBtn)
    {
        yield return new WaitUntil(() => isRotateStarted == false);
        if (faceDetectors[2].GetChild(4).GetComponent<SearchStringHelper>().SearchString != "F") //doðru çözüm için gerekli olan front face orta küpü düzeltiyoruz.
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
        }
        else if (faceDetectors[0].GetChild(4).GetComponent<SearchStringHelper>().SearchString != "U") //doðru çözüm için gerekli olan top face orta küpü düzeltiyoruz.
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
        }
        else
        {
            string solution = PlayerPrefs.GetString("Solution");
            if (solution == "")
            {
                solution = solve_Btn.GetComponent<Solve_Btn>().getSolution();
                PlayerPrefs.SetString("Solution", solution);
                PlayerPrefs.Save();
            }
            rotateForKociemba(solution);
        }
    }
}

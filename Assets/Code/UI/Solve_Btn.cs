using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;
using UnityEngine.UI;

public class Solve_Btn : MonoBehaviour
{
    public List<Transform> faceDetectors;
    public Transform rubicCube;
    public Button solve_Btn;

    //küpümüzün up=> beyaz
    //küpümüzün front=> mavi
    //küpümüzün left=> sarý
    //küpümüzün right=> kýrmýzý
    //küpümüzün back=> turuncu
    //küpümüzün down=> yeþil
    //search string yazýlma sýrasý Up,Right,Front,Down,Left,Back

    public void onClick()
    {
        solve_Btn.interactable = false;

        string solution = getSolution();
        if (solution.Contains("Error"))
        {
            PlayerPrefs.DeleteKey("Solution");
            StartCoroutine(shuffleOneStepForEveryTouch(solution));
        }
        else
        {
            rubicCube.GetComponent<CubeControl>().rotateAndFixCubeForKociembaStart(faceDetectors, transform);
        }
    }

    IEnumerator shuffleOneStepForEveryTouch(string solution)
    {
        int orjinalShuffleCount = rubicCube.GetComponent<CubeControl>().shuffleStepCount;
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = 1;
        rubicCube.GetComponent<CubeControl>().shuffleCube(solve_Btn);
        yield return new WaitUntil(() => rubicCube.GetComponent<CubeControl>().isRotateStarted == false);
        solution = getSolution();
        solve_Btn.interactable = true;
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = orjinalShuffleCount;
    }

    public string getSolution()
    {
        string searchString = "";
        foreach (var item in faceDetectors)
        {
            searchString += item.GetComponent<CubeFaceColorDetect>().getSearchSubStrings();
        }

        string info = "";
        string solution = SearchRunTime.solution(searchString, out info);
        return solution;
    }
}

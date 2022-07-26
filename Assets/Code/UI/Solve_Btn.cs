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

    //k�p�m�z�n up=> beyaz
    //k�p�m�z�n front=> mavi
    //k�p�m�z�n left=> sar�
    //k�p�m�z�n right=> k�rm�z�
    //k�p�m�z�n back=> turuncu
    //k�p�m�z�n down=> ye�il
    //search string yaz�lma s�ras� Up,Right,Front,Down,Left,Back

    public void onClick()
    {
        string solution = getSolution();
        if (solution.Contains("Error"))
        {
            StartCoroutine(shuffleOneStep(solution));
        }
        else
        {
            rubicCube.GetComponent<CubeControl>().rotateAndFixCubeForKociembaStart(faceDetectors, transform);
        }
    }

    IEnumerator shuffleOneStep(string solution)
    {
        int orjinalShuffleCount = rubicCube.GetComponent<CubeControl>().shuffleStepCount;
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = 1;
        while (solution.Contains("Error"))
        {
            Debug.Log("shuffle");
            rubicCube.GetComponent<CubeControl>().shuffleCube(solve_Btn);
            yield return new WaitUntil(() => rubicCube.GetComponent<CubeControl>().isRotateStarted == false);

            solution = getSolution();
        }
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = orjinalShuffleCount;

        rubicCube.GetComponent<CubeControl>().rotateAndFixCubeForKociembaStart(faceDetectors, transform);
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

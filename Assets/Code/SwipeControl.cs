using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
    }

    // Update is called once per frame
    void Update()
    {
        //if(SwipeManager.IsSwipingDown())
        //{
        //    Debug.Log("aþaðý kaydýrma");
        //}
    }
    private void OnSwipeDetected(Swipe swipeDirection, Vector2 swipeVelocity)
    {
        Debug.Log(swipeDirection);
    }

}

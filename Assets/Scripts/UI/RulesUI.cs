using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesUI : MonoBehaviour
{
    public GameObject swipeObject;
    public float swipeSpeed = 5f;
    public float targetPosition;
    public float swipeDistance;
    public float maxLeftPosition = -950f;
    public float maxRightPosition = -4750f;
    private void Awake()
    {
        targetPosition = swipeObject.transform.position.x;
    }
    private void Update()
    {
        float step = swipeSpeed * Time.deltaTime;
        Vector3 target = new Vector3(targetPosition, swipeObject.transform.position.y, swipeObject.transform.position.z);
        swipeObject.transform.position = Vector3.MoveTowards(swipeObject.transform.position, target, step);
    }

    public void SwipeLeft()
    {
        if (targetPosition<= maxLeftPosition)
        {
            targetPosition += swipeDistance;
        }
    }

    public void SwipeRight()
    {
        if (targetPosition >= maxRightPosition)
        {
            targetPosition -= swipeDistance;
        }
    }
}
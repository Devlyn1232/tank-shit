using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLocation : MonoBehaviour
{
    public GameObject wayPoint;
    private float timer = 0.1f;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            UpdatePosition();
            timer = 0.01f;
        }
    }

    void UpdatePosition()
    {
        wayPoint.transform.position = transform.position;
    }
}
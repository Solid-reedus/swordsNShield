using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedManager : MonoBehaviour
{
    //this code changes the gamespeed
    public float speedState = 1;
    void Update()
    {
        Time.timeScale = speedState;
    }
}

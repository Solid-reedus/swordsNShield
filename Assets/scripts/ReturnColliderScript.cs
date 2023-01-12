using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnColliderScript : MonoBehaviour
{
    public Collider Collider;

    private void OnTriggerEnter(Collider col)
    {
        Collider = col;
    }

    private void OnTriggerExit(Collider other)
    {
        //Collider = null;
    }

}

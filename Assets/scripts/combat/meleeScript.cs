using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeScript : MonoBehaviour
{
    public Imelee Imelee;

    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Imelee = GetComponent<Imelee>();
    }

    public void SwingSword()
    {
        Debug.Log("Imelee.dir = " + Imelee.lookVal);

        switch (Imelee.lookVal)
        {
            case 1:
            {
                animator.Play("swing one handed R");
                break;
            }
            case 2:
            {

                break;
            }
            case 3:
            {
                animator.Play("swing one handed L");
                break;
            }
            case 4:
            {

                break;
            }


            default:
                break;
        }
    }


}

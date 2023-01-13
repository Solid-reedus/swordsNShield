using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class blockScript : MonoBehaviour
{
    public IBlock IBlock;
    [SerializeField] Transform riggingTarget;

    [SerializeField] private Transform blockTransformR;
    [SerializeField] private Transform blockTransformB;
    [SerializeField] private Transform blockTransformL;
    [SerializeField] private Transform blockTransformT;

    //bool movingshield = false;

    [SerializeField] private float blockTime = 0.4f;
    [SerializeField] private TwoBoneIKConstraint Constraint;

    /*
    private void Update()
    {
        Debug.Log("iblock mesh = " + IBlock.shieldTrigger);
    }
    */

    private void Start()
    {
        Constraint = GetComponentInChildren<TwoBoneIKConstraint>();
        IBlock = GetComponentInChildren<IBlock>();
        Constraint.weight = 0;
    }

    IEnumerator UnBlockLerp()
    {
        float time = 0;
        float var;
        while (time < blockTime)
        {
            var = Mathf.Lerp(0, 1, time / blockTime);
            Constraint.weight = 1 - var;
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator BlockLerp()
    {
        float time = 0;
        while (time < blockTime)
        {
            Constraint.weight = Mathf.Lerp(0, 1, time / blockTime);
            time += Time.deltaTime;

            yield return null;
        }
    }

    public void Block(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            StartCoroutine(BlockLerp());
            IBlock.isBlocking = true;
        }

        if (IBlock.isBlocking == false)
        {
            switch (IBlock.lookVal)
            {
                case 1:
                {
                    riggingTarget.position = blockTransformR.position;
                    riggingTarget.eulerAngles = blockTransformR.eulerAngles;
                    break;
                }
                case 2:
                {
                    riggingTarget.position = blockTransformB.position;
                    riggingTarget.eulerAngles = blockTransformB.eulerAngles;
                    break;
                }
                case 3:
                {
                    riggingTarget.position = blockTransformL.position;
                    riggingTarget.eulerAngles = blockTransformL.eulerAngles;
                    break;
                }
                case 4:
                {
                    riggingTarget.position = blockTransformT.position;
                    riggingTarget.eulerAngles = blockTransformT.eulerAngles;
                    break;
                }
                default:
                    break;
            }
        }

        if (context.canceled)
        {
            StartCoroutine(UnBlockLerp());
            IBlock.isBlocking = false;
        }


    }

}

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

    [SerializeField] private TwoBoneIKConstraint Constraint;

    private void Start()
    {
        Constraint = GetComponentInChildren<TwoBoneIKConstraint>();
        IBlock = GetComponentInChildren<IBlock>();
        Constraint.weight = 0;
    }

    public void Block(InputAction.CallbackContext context )
    {
        //Debug.Log("context = " + context);

        Constraint.weight = 1;

        //Debug.Log("IBlock.lookVal = " + IBlock.lookVal);

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

        if (context.canceled)
        {
            Constraint.weight = 0;
        }


    }

}

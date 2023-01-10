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

    [SerializeField] private float blockTime = 33;
    [SerializeField] private TwoBoneIKConstraint Constraint;

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
            time++;
            var = Mathf.Lerp(0, 1, time / blockTime);
            Constraint.weight = 1 - var;
            yield return null;
        }
    }

    IEnumerator BlockLerp()
    {
        float time = 0;
        while (time < blockTime)
        {
            time++;
            Constraint.weight = Mathf.Lerp(0, 1, time / blockTime);
            
            yield return null;
        }
    }

    public void Block(InputAction.CallbackContext context )
    {

        if (context.performed)
        {
            StartCoroutine(BlockLerp());
            IBlock.movingshield = true;
        }

        if (IBlock.movingshield == false)
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
            IBlock.movingshield = false;
        }


    }

}

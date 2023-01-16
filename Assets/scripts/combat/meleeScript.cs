using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeScript : MonoBehaviour
{
    public Imelee Imelee;
    public IBlock IBlock;

    [SerializeField] private float damage = 35;
    bool hasSwung = false;
    bool isHit = false;

    private string animName = "sword swing";
    public string animNameR; 
    public string animNameB; 
    public string animNameT; 
    public string animNameL; 

    //private Collider SwordTrigger;

    [SerializeField] private ReturnColliderScript colScript;
    [SerializeField] private Animator animator;
    public Dictionary<string, float> animList = new Dictionary<string, float>();

    private void Awake()
    {
        animNameR = animName + " R";
        animNameB = animName + " B";
        animNameT = animName + " T";
        animNameL = animName + " L";

        Imelee = GetComponentInChildren<Imelee>();
        IBlock = GetComponentInChildren<IBlock>();
        animator = GetComponentInChildren<Animator>();
        colScript = GetComponentInChildren<ReturnColliderScript>();

        foreach (var item in animator.runtimeAnimatorController.animationClips)
        {
            animList.Add(item.name, item.length);
        }
    }

    IEnumerator WaitForSecondSwing(float dur)
    {
        yield return new WaitForSeconds(dur + 0.25f);
        isHit = false;
        hasSwung = false;
        yield return null;
    }

    IEnumerator SwingSwordCoroutine(float dur)
    {
        Debug.Log("SwingSwordCoroutine is run");
        dur *= 100; // ~150
        float time = 0;

        while (time < dur && !isHit)
        {
            time++;
            if (colScript.Collider != null
                && !isHit
                && time > dur * 0.3
                && time < dur * 0.8)
            {
                if (colScript.Collider.GetComponent<IdamageAble>() != null)
                {
                    IdamageAble damageAble = colScript.Collider.GetComponent<IdamageAble>();
                    damageAble.damage(damage);
                    colScript.Collider = null;
                    Debug.Log($"isHit = {isHit} damageAble = {damageAble}");
                    isHit = true;
                    //yield break;
                }
                else if (colScript.Collider.tag == "shield")
                {
                    Debug.Log($"shield = {colScript.Collider.name}");
                    isHit = true;
                }
            }
            yield return null;
        }
    }

    public void SwingSword()
    {
        if (IBlock == null)
        {
            return;
        }

        if (!IBlock.isBlocking && !hasSwung)
        {
            switch (Imelee.lookVal)
            {
                case 1:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameR]));
                    StartCoroutine(WaitForSecondSwing(animList[animNameR]));
                    animator.Play(animNameR);
                    break;
                }
                case 2:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameT]));
                    StartCoroutine(WaitForSecondSwing(animList[animNameT]));
                    animator.Play(animNameT);
                    break;
                }
                case 3:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameL]));
                    StartCoroutine(WaitForSecondSwing(animList[animNameL]));
                    animator.Play(animNameL);
                    break;
                }
                case 4:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameB]));
                    StartCoroutine(WaitForSecondSwing(animList[animNameB]));
                    animator.Play(animNameB);
                    break;
                }
                default:
                    break;
            }
        }
    }
}

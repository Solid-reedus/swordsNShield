using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeScript : MonoBehaviour
{
    public Imelee Imelee;
    public IBlock IBlock;

    //[SerializeField] private float damage = 35;
    float minDamage = 20;
    float maxDamage = 45;
    //bool hasSwung = false;
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
        Imelee.isSwinging = false;
        yield return null;
    }

    IEnumerator SwingSwordCoroutine(float dur, float start, float end)
    {
        Imelee.isSwinging = true;
        dur *= 100; // ~150
        float time = 0;

        while (time < dur && !isHit)
        {
            time++;
            if (colScript.Collider != null
                && !isHit
                && time > dur * start
                && time < dur * end
                )
            {
                //Debug.Log($"colScript.Collider = {colScript.Collider}");

                if (colScript.Collider.GetComponent<IdamageAble>() != null 
                    && colScript.Collider.tag == Imelee.enemyTag)
                {
                    Debug.Log($"enemy = {colScript.Collider.name}");
                    IdamageAble damageAble = colScript.Collider.GetComponent<IdamageAble>();
                    float dmg = Random.Range(minDamage, maxDamage);
                    damageAble.damage(dmg);
                    colScript.Collider = null;
                    //Debug.Log($"isHit = {isHit} damageAble = {damageAble}");
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

        /*
          S  E   T  | S    E
        -----------------------
        L 5  16  46 | 0.1  0.35
        R 18 24  43 | 0.4  0.55
        B 15 21  28 | 0.54 0.75
        T 31 41  56 | 0.55 0.73
        */

        if (!IBlock.isBlocking && !Imelee.isSwinging)
        {
            switch (Imelee.lookVal)
            {
                case 1:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameR], 0.1f , 0.35f));
                    StartCoroutine(WaitForSecondSwing(animList[animNameR]));
                    animator.Play(animNameR);
                    break;
                }
                case 2:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameT], 0.4f, 0.55f));
                    StartCoroutine(WaitForSecondSwing(animList[animNameT]));
                    animator.Play(animNameT);
                    break;
                }
                case 3:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameL], 0.54f, 0.75f));
                    StartCoroutine(WaitForSecondSwing(animList[animNameL]));
                    animator.Play(animNameL);
                    break;
                }
                case 4:
                {
                    StartCoroutine(SwingSwordCoroutine(animList[animNameB], 0.55f, 0.73f));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeScript : MonoBehaviour
{
    public Imelee Imelee;
    public IBlock IBlock;

    float minDamage = 20;
    float maxDamage = 45;
    bool isHit = false;

    private string animName = "sword swing";
    public string animNameR; 
    public string animNameB; 
    public string animNameT; 
    public string animNameL; 

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

        //this code adds entries into the animList Dictionary
        foreach (var item in animator.runtimeAnimatorController.animationClips)
        {
            animList.Add(item.name, item.length);
        }

    }

    //this coroutine prevents the player or ai to swing their sword rapidly
    IEnumerator WaitForSecondSwing(float dur)
    {
        yield return new WaitForSeconds(dur + 0.25f);
        isHit = false;
        Imelee.isSwinging = false;
        yield return null;
    }

    /*
    this Coroutine plays a swing animation based on the direction value
    and checks for collisions between 2 specified values (float start, float end) a start 
    and end value.
    this can be used when a sword swing should and shouldnt count
    */
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
                if (colScript.Collider.GetComponent<IdamageAble>() != null 
                    && colScript.Collider.tag == Imelee.enemyTag)
                {
                    IdamageAble damageAble = colScript.Collider.GetComponent<IdamageAble>();
                    float dmg = Random.Range(minDamage, maxDamage);
                    damageAble.damage(dmg);
                    colScript.Collider = null;
                    isHit = true;
                }
                else if (colScript.Collider.tag == "shield")
                {
                    isHit = true;
                }
            }
            yield return null;
        }
    }

    /*
    this code when called (by player or ai) if not blocking will start the SwingSwordCoroutine
    and WaitForSecondSwing Coroutines.
    the one Coroutine will register a hit and the other will count down for the next possible swing
    */
    public void SwingSword()
    {
        if (IBlock == null)
        {
            return;
        }

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

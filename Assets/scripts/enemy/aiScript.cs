using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiScript : MonoBehaviour, IdamageAble, Imelee, IBlock
{
    [SerializeField] private GameObject armour;
    [SerializeField] private GameObject shield;
    [SerializeField] private Material enemyMaterial;

    AiblockScript AiblockScript;
    NavMeshAgent NavMeshAgent;
    SpawnManager SpawnManager;
    meleeScript meleeScript;
    GameObject target;
    Animator Animator;

    [SerializeField] private bool isTryingToAttack = false;
    [SerializeField] private bool isTryingToBlock = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isBlocking = false;

    private int enemyStateVal = 1;
    [SerializeField] private float health = 100;
    private int lookval = 3; 
    private string enemyTag;
    private bool battleIsOver = false;

    public bool isDead = false;

    int IdirectionalInput.lookVal { get { return lookval; } }
    bool Imelee.isSwinging { get { return isAttacking; } set { this.isAttacking = value; } }
    string Imelee.enemyTag { get { return enemyTag; } set { this.enemyTag = value; } }
    bool IBlock.isBlocking { get { return isBlocking; } set { this.isBlocking = value; } }
    Collider IBlock.shieldTrigger { get { return shield.GetComponent<Collider>(); } }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        SpawnManager = FindObjectOfType<SpawnManager>();
        Animator = GetComponentInChildren<Animator>();
        meleeScript = GetComponent<meleeScript>();
        AiblockScript = GetComponent<AiblockScript>();

        if (tag == "ally")
        {
            enemyTag = "enemy";
        }
        else if (tag == "enemy")
        {
            enemyTag = "ally";
        }
        else
        {
            Debug.LogError($"{this.name} isnt asignt a side");
        }
        Colorcoat();
        target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
    }

    void Colorcoat()
    {
        if (this.tag == "ally")
        {
            shield.transform.GetChild(0).gameObject.SetActive(true);
            shield.transform.GetChild(1).gameObject.SetActive(false);
            return;
        }

        armour.GetComponent<SkinnedMeshRenderer>().material = enemyMaterial;
        shield.transform.GetChild(0).gameObject.SetActive(false);
        shield.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
        }
    }

    void Update()
    {
        if (health > 1)
        {
            KnightBlock();
            KnightNav();
            KnightAttack();
            KnightAnim();
        }
    }

    void KnightAnim()
    {
        if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance
            || !battleIsOver
            || target == null)
        {
            Animator.SetFloat("inputY", 1);
        }
        else
        {
            Animator.SetFloat("inputY", 0);
        }
    }

    public void damage(float dmg)
    {
        health -= dmg;
        Animator.Play("hit");
        if (health < 1)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Animator.applyRootMotion = true;
        Animator.Play("no more walking");
        Animator.Play("die");
        isDead = true;
        SpawnManager.BattleIsOver(enemyTag);
    }

    private void KnightNav()
    {
        switch (enemyStateVal)
        {
            // idle
            case 1:
            {
                if (SpawnManager.BattleIsOver(enemyTag))
                {
                    battleIsOver = true;
                    if (!isDead)
                    {
                        Animator.Play("wave arm");
                    }
                    enemyStateVal = 5;
                    break;
                }
                enemyStateVal = 2;
                break;
            }
            // choose target
            case 2:
            {
                target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
                enemyStateVal = 3;
                break;
            }
            // go to target
            case 3:
            {
                if (target == null)
                {
                    enemyStateVal = 1;
                    break;
                }
                NavMeshAgent.SetDestination(target.transform.position);
                this.transform.LookAt(target.transform);
                break;
            }

            // ! work in progress !
            // back off
            case 4:
            {
                if (NavMeshAgent.remainingDistance > 1.5)
                {
                    NavMeshAgent.SetDestination(target.transform.position);
                    enemyStateVal = 3;
                    break;
                }
                /*
                //local vec to glo
                Vector3 goBack = this.transform.InverseTransformDirection(Vector3.forward);
                goBack.x -= 10;
                Debug.Log($"goBack = {goBack} and transformpoint {transform.TransformPoint(goBack)}");
                NavMeshAgent.SetDestination(transform.TransformPoint(goBack));
                */

                    //transform.TransformPoint

                    //NavMeshAgent.SetDestination(-target.transform.position);
                    // start a couroutine when a sword is swung after a random amount of seconds to block  
                    break;
            }
            case 5:
            {
                break;
            }

        }


    }

    void KnightAttack()
    {
        if (NavMeshAgent.remainingDistance < 1.5f 
            && !isTryingToAttack
            && target != null)
        {
            
            //Debug.Log("knight is in range");
            float newAttack = Random.Range(0.3f, 3.5f);
            lookval = Random.Range(1, 5);

            float dur = 1;


            switch (lookval)
            {
                case 1:
                {
                    dur = meleeScript.animList[meleeScript.animNameR];
                    break;
                }
                case 2:
                {
                    dur = meleeScript.animList[meleeScript.animNameB];
                    break;
                }
                case 3:
                {
                    dur = meleeScript.animList[meleeScript.animNameL];
                    break;
                }
                case 4:
                {
                    dur = meleeScript.animList[meleeScript.animNameT];
                    break;
                }
                default:
                    break;
            }

            dur += 0.5f;

            StartCoroutine(SwordTimer(newAttack , dur));
            isTryingToAttack = true;
            //Debug.Log(lookval);
            
        }
        //yield return new WaitForSeconds(newAttack);
    }
    
    IEnumerator SwordTimer(float delay ,float timerToWait)
    {
        yield return new WaitForSeconds(delay);

        meleeScript.SwingSword();

        yield return new WaitForSeconds(timerToWait);

        if (target.GetComponent<aiScript>() != null)
        {
            if (target.GetComponent<aiScript>().isDead)
            {
                target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
            }
        }
        else if (target.GetComponent<playerInput>().isDead)
        {
            target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
        }
        else
        {
            enemyStateVal = 1;
        }

        if (target == null)
        {
            enemyStateVal = 1;
        }
        isTryingToAttack = false;
        isAttacking = false;
    }

    void KnightBlock()
    {
        if (target == null || isBlocking)
        {
            return;
        }

        if (target.gameObject.GetComponent<Imelee>().isSwinging && !isTryingToBlock)
        {
            float reactionTime = Random.Range(0.2f, 0.7f);
            float dur = Random.Range(1.2f, 2.6f);
            isTryingToBlock = true;

            StartCoroutine(SHieldTimer(reactionTime, dur));
        }
        
    }

    IEnumerator SHieldTimer(float reactionTime, float dur)
    {
        yield return new WaitForSeconds(reactionTime);

        int chance = Random.Range(1, 4);
        if (chance != 1)
        {
            int dir = target.GetComponent<IdirectionalInput>().lookVal;
            isBlocking = true;
            StartCoroutine(AiblockScript.block(dir));
            Debug.Log("I am going to block");
        }

        yield return new WaitForSeconds(dur);
        isTryingToBlock = false;
        isBlocking = false;
    }
    
}

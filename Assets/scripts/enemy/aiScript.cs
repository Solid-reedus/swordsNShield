using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiScript : MonoBehaviour, IdamageAble, Imelee, IBlock
{
    [SerializeField] private GameObject armour;
    [SerializeField] private GameObject shield;
    [SerializeField] private Material enemyMaterial;
    //[SerializeField] private Collider collider;
    private Animator Animator;

    NavMeshAgent NavMeshAgent;
    meleeScript meleeScript;
    GameObject target;

    private int lookval = 1; 

    [SerializeField] private int enemyStateVal = 1;
    [SerializeField] private float health = 100;
    SpawnManager SpawnManager;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isBlocking = false;

    bool canSwing = true;
    public bool isDead = false;

    string enemyTag;

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
        //NavMeshAgent.stoppingDistance = 1.3f;

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

    void Update()
    {
        if (health > 1)
        {
            KnightNav();
            KnightAttack();
        }
    }

    public void damage(float dmg)
    {
        health -= dmg;
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
    }

    IEnumerator Timer(bool TimerBool, float timerToWait)
    {
        //TimerBool = true;
        yield return new WaitForSeconds(timerToWait);
        //TimerBool = false;
        isAttacking = false;
    }

    void KnightAttack()
    {
        if (NavMeshAgent.remainingDistance < 1.5f && !isAttacking)
        {
            isAttacking = true;
            //Debug.Log("knight is in range");
            float newAttack = Random.Range(0.1f, 1.5f);
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

            StartCoroutine(Timer(isAttacking, dur));
            //Debug.Log(lookval);
            meleeScript.SwingSword();
        }
        //yield return new WaitForSeconds(newAttack);
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

                //StartCoroutine(GoToEnemy(
                //SpawnManager.GetClosestEnemy(this.transform, enemyTag)));
                // go position of the player unit it hits a trigger
            }
            // go to target
            case 3:
            {
                //target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
                if (target == null)
                {
                    enemyStateVal = 1;
                    break;
                }
                NavMeshAgent.SetDestination(target.transform.position);
                //Debug.Log("remainingDistance =" + NavMeshAgent.remainingDistance);
                this.transform.LookAt(target.transform);

                /*
                if (!isAttacking && NavMeshAgent.remainingDistance < 1)
                {
                    Vector3 goBack = this.transform.InverseTransformDirection(Vector3.forward);
                    goBack.x -= 10;
                    Debug.Log($"goBack = {goBack} and transformpoint {transform.TransformPoint(goBack)}");
                    NavMeshAgent.SetDestination(transform.TransformPoint(goBack));
                    enemyStateVal = 4;
                }
                */
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

        }


    }

    
}

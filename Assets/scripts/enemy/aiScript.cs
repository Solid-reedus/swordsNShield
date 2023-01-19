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

    /*
    when the AI is "awake" it will define who the enemy is based on its own tag
    if the AI is a "enemy" it will also recolor it self with red
    */
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

    //this code gets the body armour and shield and recolors it
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

    // this code will retargert a target when it collides with a enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
        }
    }

    //if the enemy is alive it will prefroms the listed methods
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

    /*
    this code sets the walking varables to 1 (walk forward) if the NavMeshAgents distance from
    the target is bigger than the stoping its distance,
    the battle isnt over or there isnt a target
    */
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

    /*
    this is a implementation of the IdamageAble interface and
    allows this method to be called as a interface instead the whole script
    this script damages the AI
    */
    public void damage(float dmg)
    {
        health -= dmg;
        Animator.Play("hit");
        if (health < 1)
        {
            Die();
        }
    }

    //if the AI is dead it will disable most things and set the isDead bool to true
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

    /*
    the knight has 4 states
    1 - idle and wave arm if the AI has won
    2 - choose a target if there are any
    3 - as long as the knight has target that isnt death it will follow it
    4 - do nothing
    */
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
                    enemyStateVal = 4;
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
            case 4:
            {
                break;
            }
        }
    }

    /*
    if the knight is in range, isnt already attacking and has a target
    he will attack
    based on newAttack
    and will choose direction based an random number between 1 and 4.
    and will start the SwordTimer coroutine
    */
    void KnightAttack()
    {
        if (NavMeshAgent.remainingDistance < 1.5f 
            && !isTryingToAttack
            && target != null)
        {
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
        }
    }

    //this timer will keep track of the knights atempt to swing his sword and the swing it self
    //to separate attack between each other
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

    /*
    if the enemy isnt already swinging its sword it will try to block the enemies attack
    by gettings the enemies sword swing direction with a delay and a block time 
    */
    void KnightBlock()
    {
        if (target == null || isBlocking)
        {
            return;
        }

        if (target.gameObject.GetComponent<Imelee>().isSwinging && !isTryingToBlock)
        {
            //this the AI's reaction time
            float reactionTime = Random.Range(0.2f, 0.7f);
            //this is the duration of the block
            float dur = Random.Range(1.2f, 2.6f);
            isTryingToBlock = true;

            StartCoroutine(ShieldTimer(reactionTime, dur));
        }
        
    }

    //this coroutine will try (2/3 chance by design) to block the enemy sword swing
    IEnumerator ShieldTimer(float reactionTime, float dur)
    {
        yield return new WaitForSeconds(reactionTime);
        //this code adds a 1/3 chance the enemy wont block at all
        int chance = Random.Range(1, 4);
        if (chance != 1)
        {
            int dir = target.GetComponent<IdirectionalInput>().lookVal;
            isBlocking = true;
            StartCoroutine(AiblockScript.block(dir));
        }

        yield return new WaitForSeconds(dur);
        isTryingToBlock = false;
        isBlocking = false;
    }
    
}

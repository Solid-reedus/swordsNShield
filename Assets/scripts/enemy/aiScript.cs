using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiScript : MonoBehaviour, IdamageAble
{
    [SerializeField] private GameObject armour;
    [SerializeField] private GameObject shield;
    [SerializeField] private Material enemyMaterial;
    private Animator Animator;

    NavMeshAgent NavMeshAgent;
    GameObject target;


    [SerializeField] private int enemyStateVal = 1;
    //private int enemyWeaponEquip = 1;
    //string tag = "";
    [SerializeField] private float health = 100;
    SpawnManager SpawnManager;

    /*
    bool canWalk = true;
    bool canSwing = true;
    bool canBlock = true;
    */

    string enemyTag;


    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        SpawnManager = FindObjectOfType<SpawnManager>();
        Animator = GetComponentInChildren<Animator>();

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

    
    IEnumerator GoToEnemy(GameObject target)
    {
        if ( Vector3.Distance(this.transform.position , target.transform.position) < 2.5f)
        {
            enemyStateVal = 3;
            yield break;
        }
        //Debug.Log($"{this.name} is following {target} at {target.transform.position}");
        NavMeshAgent.SetDestination(target.transform.position);

        //yield break;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (health > 1)
        {
            KnightState();
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
        //Animator.applyRootMotion = true;
        Animator.Play("die");

    }

    private void KnightState()
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
            // chase
            case 2:
            {
                StartCoroutine(GoToEnemy(
                SpawnManager.GetClosestEnemy(this.transform, enemyTag)));
                // go position of the player unit it hits a trigger
                break;
            }
            // REselect target
            case 3:
            {
                target = SpawnManager.GetClosestEnemy(this.transform, enemyTag);
                this.transform.LookAt(target.transform);
                enemyStateVal = 4;
                break;
            }


            // after a random amount of seconds start a couroutine to swing a sword 
            // block attack
            case 4:
            {
                // start a couroutine when a sword is swung after a random amount of seconds to block  
                break;
            }
            // die
            case 5:
            {
                // when the enemy health is 0 or less the enemy dies
                break;
            }


        }


    }

    
}

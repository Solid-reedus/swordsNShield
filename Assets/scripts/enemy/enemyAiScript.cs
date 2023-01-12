using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAiScript : MonoBehaviour, IdamageAble
{
    private int enemyStateVal = 1;
    //private int enemyWeaponEquip = 1;

    [SerializeField] private float health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damage(float dmg)
    {
        health -= dmg;
    }

    private void enemyState()
    {
        switch (enemyStateVal)
        {
            // idle
            case 1:
            {
                break;
            }
            // chase
            case 2:
            {
                // go position of the player unit it hits a trigger
                break;
            }
            // swing sword
            case 3:
            {
                // after a random amount of seconds start a couroutine to swing a sword 
                break;
            }
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

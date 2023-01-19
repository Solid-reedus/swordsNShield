using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private int allyCount  = 5;

    [SerializeField] private Transform enemySpawnA;
    [SerializeField] private Transform enemySpawnB;

    [SerializeField] private Transform AllySpawnA;
    [SerializeField] private Transform AllySpawnB;

    [SerializeField] private GameObject Knight;
    [SerializeField] private sceneManager sceneManager;


    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();

    public int WhoWon = 0;

    void Start()
    {
        //if there isnt a enemyCount or allyCount it will define itself
        if (sceneManager != null)
        {
            enemyCount = sceneManager.enemyCount;
            allyCount = sceneManager.allyCount;
        }

        SpawnKnight("enemy", enemyCount, enemySpawnA.position, enemySpawnB.position);
        SpawnKnight("ally", allyCount, AllySpawnA.position, AllySpawnB.position);

        //the 2 GameObject[]'s are temporary GameObject[]'s that are convertert into list's
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] allAllies = GameObject.FindGameObjectsWithTag("ally");

        foreach (var item in allEnemies)
        {
            enemies.Add(item);
        }
        foreach (var item in allAllies)
        {
            allies.Add(item);
        }
    }

    /*
    this methods starts with a bool battleIsOver that is set true.
    if there is any enemy alive (ally for the enemy) then it is set to false
    */
    public bool BattleIsOver(string tagName)
    {
        // this bool true unless it is truned false by getting all alive enemies/allies
        //
        bool battleIsOver = true;

        if (tagName == "enemy")
        {
            foreach (var item in enemies)
            {
                if (item != null)
                {
                    if (item.GetComponent<aiScript>() != null)
                    {
                        if (!item.GetComponent<aiScript>().isDead)
                        {
                            battleIsOver = false;
                            return battleIsOver;
                        }
                    }
                }
            }
            WhoWon = 1;
        }
        else if (tagName == "ally")
        {
            foreach (var item in allies)
            {
                if (item.GetComponent<aiScript>() != null)
                {
                    if (!item.GetComponent<aiScript>().isDead)
                    {
                        battleIsOver = false;
                        return battleIsOver;
                    }
                }
                else if (item.GetComponent<playerInput>() != null)
                {
                    if (!item.GetComponent<playerInput>().isDead)
                    {
                        battleIsOver = false;
                        return battleIsOver;
                    }
                }
            }
            WhoWon = 2;
        }
        else
        {
            Debug.LogError("unusable tag");
        }
        return battleIsOver;
    }

    /*
    this method spawns knights based of "amount" 
    and will be fight  for the team that "tagName" specifies.
    the spawn area is between spawnP1 and spawnP2
    */
    private void SpawnKnight(string tagName, int amount, Vector3 spawnP1, Vector3 spawnP2)
    {
        List<Vector3> spawnList = new List<Vector3>();

        Knight.tag = tagName;
        for (int i = 0; i < amount; i++)
        {
            Instantiate(Knight, makeNewSpawn(), Quaternion.Euler(0,0,0));
        }

        Vector3 makeNewSpawn()
        {
            float dis;
            bool isOccupied = false;
            float posZ = Random.Range(spawnP1.z, spawnP2.z);
            float posX = Random.Range(spawnP1.x, spawnP2.x);
            Vector3 spawn = new Vector3(posX, 0, posZ);

            foreach (var item in spawnList)
            {
                dis = Vector3.Distance(spawn, item);
                if (dis > 1)
                {
                    isOccupied = true;
                    break;
                }
            }
            if (isOccupied == true)
            {
                makeNewSpawn();
            }
            return spawn;
        }

    }

    //this method will go through the list of the enemy and get the closest one based on distance
    public GameObject GetClosestEnemy(Transform instigator, string tagName)
    {
        //closest is a high number so it will always be the biggest number
        float closest = 100;
        float tempVal;
        GameObject target = null;

        if (tagName == "enemy")
        {
            foreach (var item in enemies)
            {
                tempVal = Vector3.Distance(instigator.position, item.transform.position);
                if (item.GetComponent<aiScript>() != null)
                {
                    if (!item.GetComponent<aiScript>().isDead)
                    {
                        //Debug.Log("enemy found =" + item);
                        closest = tempVal;
                        target = item;
                    }
                }
            }
        }
        else if (tagName == "ally")
        {
            foreach (var item in allies)
            {
                tempVal = Vector3.Distance(instigator.position, item.transform.position);

                if (item.GetComponent<aiScript>() != null)
                {
                    if (!item.GetComponent<aiScript>().isDead)
                    {
                        //Debug.Log("enemy found =" + item);
                        closest = tempVal;
                        target = item;
                    }
                }
                else if(item.GetComponent<playerInput>() != null)
                {
                    if (!item.GetComponent<playerInput>().isDead)
                    {
                        closest = tempVal;
                        target = item;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("unusable tag");
        }
        return target;
    }

}

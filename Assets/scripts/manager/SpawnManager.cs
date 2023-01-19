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
        if (sceneManager != null)
        {
            enemyCount = sceneManager.enemyCount;
            allyCount = sceneManager.allyCount;
        }

        SpawnKnight("enemy", enemyCount, enemySpawnA.position, enemySpawnB.position);
        SpawnKnight("ally", allyCount, AllySpawnA.position, AllySpawnB.position);

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

        int count = 0;
        foreach (var item in enemies)
        {
            count++;
        }
    }

    public bool BattleIsOver(string tagName)
    {
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
                            //Debug.Log("enemy found =" + item);
                            battleIsOver = false;
                            return battleIsOver;
                        }
                    }
                }
            }
            Debug.Log("battle is over");
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
                        //Debug.Log("enemy found =" + item);
                        battleIsOver = false;
                        return battleIsOver;
                    }
                }
                else if (item.GetComponent<playerInput>() != null)
                {
                    if (!item.GetComponent<playerInput>().isDead)
                    {
                        //Debug.Log("enemy found =" + item);
                        battleIsOver = false;
                        return battleIsOver;
                    }
                }
            }
            Debug.Log("battle is over");
            WhoWon = 2;
        }
        else
        {
            Debug.LogError("unusable tag");
        }

        return battleIsOver;
    }

    private void SpawnKnight(string tagName, int amount, Vector3 spawnB1, Vector3 spawnB2)
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
            float posZ = Random.Range(spawnB1.z, spawnB2.z);
            float posX = Random.Range(spawnB1.x, spawnB2.x);
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

    public GameObject GetClosestEnemy(Transform instigator, string tagName)
    {
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
                        //Debug.Log("enemy found =" + item);
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

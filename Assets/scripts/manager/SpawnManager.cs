using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private int allyCount  = 3;

    [SerializeField] private Transform enemySpawnA;
    [SerializeField] private Transform enemySpawnB;

    [SerializeField] private Transform AllySpawnA;
    [SerializeField] private Transform AllySpawnB;

    [SerializeField] private GameObject Knight;


    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        /*
        Vector3 lenghtA = new Vector3(spawnA.position.x , 0 , spawnA.position.z);
        Vector3 lenghtB = new Vector3(spawnB.position.x , 0 , spawnA.position.z);

        Vector3 widthA = new Vector3(spawnA.position.x , 0 , spawnA.position.z);
        Vector3 widthB = new Vector3(spawnA.position.x , 0 , spawnB.position.z);

        Debug.Log($"height of spawn {Vector3.Distance(lenghtA, lenghtB)} and with is {Vector3.Distance(widthA, widthB)}");
        Debug.Log("the surface of the spawn is" + Vector3.Distance(lenghtA, lenghtB) * Vector3.Distance(widthA, widthB));
        */

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
            //Debug.Log($"num {count} enemy {enemies} pos {item.transform.position}");
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
                    //Debug.Log("enemy found =" + item);
                    battleIsOver = false;
                    return battleIsOver;
                }
            }
        }
        else if (tagName == "ally")
        {
            foreach (var item in allies)
            {
                if (item != null)
                {
                    //Debug.Log("ally found =" + item);
                    battleIsOver = false;
                    return battleIsOver;
                }
            }
        }
        else
        {
            Debug.LogError("unusable tag");
        }
        /*
        foreach (var item in enemies)
        {
            if (item != null)
            {
                battleIsOver = false;
                return battleIsOver;
            }
        }
        */
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
        //Debug.Log($"GetClosestEnemy is prefromed Transform = {instigator} tagName = {tagName}");

        float closest = 100;
        float tempVal;
        GameObject target = null;

        if (tagName == "enemy")
        {
            foreach (var item in enemies)
            {
                tempVal = Vector3.Distance(instigator.position, item.transform.position);
                if (closest > tempVal)
                {
                    closest = tempVal;
                    target = item;
                }
            }
        }
        else if (tagName == "ally")
        {
            foreach (var item in allies)
            {
                tempVal = Vector3.Distance(instigator.position, item.transform.position);
                if (closest > tempVal)
                {
                    closest = tempVal;
                    target = item;
                }
            }
        }
        else
        {
            Debug.LogError("unusable tag");
        }

        //Debug.Log($"returned value is {target.name}");
        return target;
    }

}

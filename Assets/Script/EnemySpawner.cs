using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyObj;                       //GÌIuWFNg
    private Enemy enemyScript;                                  //GÌXNvg

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnAction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawnAction() 
    {
        for(int i = 1; i < 10000; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                EnemySpawn((i * 5 + j) % 6 + 1, i * 5 + j);
                yield return null;
            }

            for (int j = 0; j < 300; j++) 
            {
                yield return null;
            }
        }
        yield break;
    }

    //GðN©¹é
    public void EnemySpawn(int iD, int level)
    {
        //GÌ¢«
        var enemyObjPrefab = Instantiate<GameObject>(enemyObj, transform.position, Quaternion.identity);
        enemyScript = enemyObjPrefab.GetComponent<Enemy>();
        //GÉîñð^¦é
        //GIDAx
        enemyScript.EnemyData(iD, level);
    }
}

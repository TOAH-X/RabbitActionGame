using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject playerObj;                      //プレイヤーオブジェクト
    [SerializeField] GameObject enemyObj;                       //敵のオブジェクト
    private Enemy enemyScript;                                  //敵のスクリプト

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnAction());
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemySpawn(0, 0, new Vector2(-65, 15));
        }
    }

    IEnumerator EnemySpawnAction() 
    {
        for(int i = 1; i < 10000; i++)
        {
            while (Vector2.Distance(playerObj.transform.position, transform.position) >= 10) 
            {
                yield return null;
            } 
            for (int j = 0; j < 5; j++)
            {
                EnemySpawn((i * 5 + j) % 6 + 1, i * 5 + j, transform.position + new Vector3(Random.Range(-1f, 1f), 0));
                yield return null;
            }

            for (int j = 0; j < 300; j++) 
            {
                yield return null;
            }
        }
        yield break;
    }

    //敵を湧かせる
    public void EnemySpawn(int iD, int level,Vector2 enemyPos)
    {
        //敵の召喚
        var enemyObjPrefab = Instantiate<GameObject>(enemyObj, enemyPos, Quaternion.identity);
        enemyScript = enemyObjPrefab.GetComponent<Enemy>();
        //敵に情報を与える
        //敵ID、レベル
        enemyScript.EnemyData(iD, level);
    }
}

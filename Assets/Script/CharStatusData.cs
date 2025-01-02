using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStatusData : MonoBehaviour
{
    [SerializeField] int[] charCurrentHpData;                   //全てのキャラの現在HPデータ
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;     //チームコントローラのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        //charCurrentHpData= new int[];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //JSONからデータを取得
    public void GetCharStatusData(int currentCharId)
    {
        
    }
}

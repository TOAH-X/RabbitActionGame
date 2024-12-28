using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //teamCoutnrollerのスクリプト

    [SerializeField] GameObject TeamChar1;                          //チームキャラ1
    [SerializeField] GameObject TeamChar2;                          //チームキャラ2
    [SerializeField] GameObject TeamChar3;                          //チームキャラ3

    private int[] teamIdData = new int[3];                          //チームデータ(ID)

    // Start is called before the first frame update
    void Start()
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

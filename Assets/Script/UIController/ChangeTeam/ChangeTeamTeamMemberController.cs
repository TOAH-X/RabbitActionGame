using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //teamCoutnrollerのスクリプト

    [SerializeField] GameObject teamChar1Obj;                       //チームキャラ1(バックグラウンド)
    [SerializeField] GameObject teamChar2Obj;                       //チームキャラ2(バックグラウンド)
    [SerializeField] GameObject teamChar3Obj;                       //チームキャラ3(バックグラウンド)

    private int[] teamIdData = new int[3];                          //チームデータ(ID)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        //ChangeTeamControllerオブジェクトの取得
        GameObject uiCanvasObj = transform.parent.parent.gameObject;
        GameObject changeTeamControllerObj = uiCanvasObj.transform.Find("ChangeTeamController").gameObject;
        ChangeTeamController changeTeamControllerObjScript = changeTeamControllerObj.GetComponent<ChangeTeamController>();


        GameObject teamChar1IconObj = teamChar1Obj.transform.Find("TeamChar1Icon").gameObject;
        Image teamChar1IconObjImage = teamChar1IconObj.GetComponent<Image>();
        teamChar1IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(teamIdData[0]);

        GameObject teamChar2IconObj = teamChar2Obj.transform.Find("TeamChar2Icon").gameObject;
        Image teamChar2IconObjImage = teamChar2IconObj.GetComponent<Image>();
        teamChar2IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(teamIdData[1]);

        GameObject teamChar3IconObj = teamChar3Obj.transform.Find("TeamChar3Icon").gameObject;
        Image teamChar3IconObjImage = teamChar3IconObj.GetComponent<Image>();
        teamChar3IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(teamIdData[2]);
    }

    //アイコンのリフレッシュ。チームメンバーを変えたとき、もしくは開いた瞬間のみ呼び出す
    public void ChangeTeamMember() 
    { 
    }
}

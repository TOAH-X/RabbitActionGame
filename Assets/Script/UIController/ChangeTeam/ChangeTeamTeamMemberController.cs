using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //TeamCoutnrollerのスクリプト

    [SerializeField] ChangeTeamController changeTeamControllerObjScript;    //ChangeTeamControllerのスクリプト

    [SerializeField] GameObject teamChar1Obj;                       //チームキャラ1(バックグラウンド)
    [SerializeField] GameObject teamChar2Obj;                       //チームキャラ2(バックグラウンド)
    [SerializeField] GameObject teamChar3Obj;                       //チームキャラ3(バックグラウンド)
    [SerializeField] GameObject cursorObj;                          //カーソルのオブジェクト

    [SerializeField] Image teamChar1IconObjImage;                   //チームキャラ1のアイコンのimage
    [SerializeField] Image teamChar2IconObjImage;                   //チームキャラ2のアイコンのimage
    [SerializeField] Image teamChar3IconObjImage;                   //チームキャラ3のアイコンのimage

    [SerializeField] RectTransform cursorRect;                      //カーソルのRectTransform
    private int currentSelectTeamId = 0;                            //現在選択中のキャラID(表で戦っているキャラのことではない)
    private int[] updateTeamId = new int[3];                        //変更後の予定のチームId

    private int[] teamIdData = new int[3];                          //チームデータ(ID)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アイコンのリフレッシュ。チームメンバーを変えたとき、もしくは開いた瞬間のみ呼び出す
    public void ChangeTeamMember() 
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        for(int i = 0; i < 3; i++) 
        {
            updateTeamId[i] = teamIdData[i];
            ChangeIcon(i, teamIdData[i]);
        }
    }

    //チーム編成更新
    public void UpdateTeam() 
    {
        teamCoutnrollerScript.ChangeTeam(updateTeamId);
    }

    //チームメンバー交代(現時点では重複した場合は操作を無効化)
    public void ChangeMember(int charId) 
    {
        int counter = 0;
        int conflictTeamId = 0;
        for (int i = 0; i < 3; i++) 
        {
            if (currentSelectTeamId != i) 
            {
                if (updateTeamId[i] != charId) 
                {
                    counter++;
                }
                else 
                {
                    conflictTeamId = i;
                }
            }
        }
        if (counter == 2) 
        {
            //キャラの交換
            updateTeamId[currentSelectTeamId] = charId;
            ChangeIcon(currentSelectTeamId, charId);
        }
        else
        {
            //重複した時のキャラの交換
            (updateTeamId[conflictTeamId], updateTeamId[currentSelectTeamId]) = (updateTeamId[currentSelectTeamId], updateTeamId[conflictTeamId]);
            ChangeIcon(conflictTeamId, updateTeamId[conflictTeamId]);
            ChangeIcon(currentSelectTeamId, updateTeamId[currentSelectTeamId]);
        }
    }

    //画像の差し替え
    public void ChangeIcon(int teamCharId, int charId) 
    {
        if (teamCharId == 0) teamChar1IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        else if (teamCharId == 1) teamChar2IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        else if (teamCharId == 2) teamChar3IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
    }

    //チームメンバー一覧から選択時
    public void SelectTeamMember(int teamId) 
    {
        currentSelectTeamId = teamId;

        //teamIdに応じて配置の変更
        cursorRect.localPosition = new Vector2(teamId * 200 - 200, 0);
    }

    //チームメンバー1のアイコンをクリックしたとき
    public void OnClickTeamMember1() => SelectTeamMember(0);

    //チームメンバー2のアイコンをクリックしたとき
    public void OnClickTeamMember2() => SelectTeamMember(1);

    //チームメンバー3のアイコンをクリックしたとき
    public void OnClickTeamMember3() => SelectTeamMember(2);

}

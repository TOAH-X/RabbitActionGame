using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeTeamIcon : MonoBehaviour, IPointerClickHandler
{
    private int charId;                             //キャラID

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(int id)
    {
        charId= id;
    }

    //アイコンがクリックされたとき
    public void OnPointerClick(PointerEventData eventData)
    {
        //CharFullBodyImageオブジェクトの取得
        GameObject changeTeamUIObj = transform.parent.parent.parent.parent.gameObject;
        GameObject charFullBodyImageObj = changeTeamUIObj.transform.Find("CharFullBodyImage").gameObject;
        //ChangeTeamControllerオブジェクトの取得
        GameObject uiCanvasObj = transform.parent.parent.parent.parent.parent.gameObject;
        GameObject changeTeamControllerObj = uiCanvasObj.transform.Find("ChangeTeamController").gameObject;
        //charFullBodyImageObjSpriteに立ち絵をセット
        Image charFullBodyImageObjSprite = charFullBodyImageObj.GetComponent<Image>();
        ChangeTeamController changeTeamControllerObjScript = changeTeamControllerObj.GetComponent<ChangeTeamController>();
        charFullBodyImageObjSprite.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        //TeamMemberオブジェクトの取得
        GameObject teamMemberObj = changeTeamUIObj.transform.Find("TeamMember").gameObject;
        ChangeTeamTeamMemberController teamMemberObjScript = teamMemberObj.GetComponent<ChangeTeamTeamMemberController>();
        teamMemberObjScript.ChangeMember(charId);
    }
}

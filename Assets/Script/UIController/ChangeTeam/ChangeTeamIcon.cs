using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeTeamIcon : MonoBehaviour, IPointerClickHandler
{
    private int charId;                             //�L����ID

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

    //�A�C�R�����N���b�N���ꂽ�Ƃ�
    public void OnPointerClick(PointerEventData eventData)
    {
        //CharFullBodyImage�I�u�W�F�N�g�̎擾
        GameObject changeTeamUIObj = transform.parent.parent.parent.parent.gameObject;
        GameObject charFullBodyImageObj = changeTeamUIObj.transform.Find("CharFullBodyImage").gameObject;
        //ChangeTeamController�I�u�W�F�N�g�̎擾
        GameObject uiCanvasObj = transform.parent.parent.parent.parent.parent.gameObject;
        GameObject changeTeamControllerObj = uiCanvasObj.transform.Find("ChangeTeamController").gameObject;
        //charFullBodyImageObjSprite�ɗ����G���Z�b�g
        Image charFullBodyImageObjSprite = charFullBodyImageObj.GetComponent<Image>();
        ChangeTeamController changeTeamControllerObjScript = changeTeamControllerObj.GetComponent<ChangeTeamController>();
        charFullBodyImageObjSprite.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        //TeamMember�I�u�W�F�N�g�̎擾
        GameObject teamMemberObj = changeTeamUIObj.transform.Find("TeamMember").gameObject;
        ChangeTeamTeamMemberController teamMemberObjScript = teamMemberObj.GetComponent<ChangeTeamTeamMemberController>();
        teamMemberObjScript.ChangeMember(charId);
    }
}

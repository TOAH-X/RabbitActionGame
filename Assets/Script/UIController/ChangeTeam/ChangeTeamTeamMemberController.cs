using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //teamCoutnroller�̃X�N���v�g

    [SerializeField] GameObject teamChar1Obj;                       //�`�[���L����1(�o�b�N�O���E���h)
    [SerializeField] GameObject teamChar2Obj;                       //�`�[���L����2(�o�b�N�O���E���h)
    [SerializeField] GameObject teamChar3Obj;                       //�`�[���L����3(�o�b�N�O���E���h)

    private int[] teamIdData = new int[3];                          //�`�[���f�[�^(ID)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        //ChangeTeamController�I�u�W�F�N�g�̎擾
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

    //�A�C�R���̃��t���b�V���B�`�[�������o�[��ς����Ƃ��A�������͊J�����u�Ԃ̂݌Ăяo��
    public void ChangeTeamMember() 
    { 
    }
}

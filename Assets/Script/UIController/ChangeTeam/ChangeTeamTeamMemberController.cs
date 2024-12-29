using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //TeamCoutnroller�̃X�N���v�g

    [SerializeField] ChangeTeamController changeTeamControllerObjScript;    //ChangeTeamController�̃X�N���v�g

    [SerializeField] GameObject teamChar1Obj;                       //�`�[���L����1(�o�b�N�O���E���h)
    [SerializeField] GameObject teamChar2Obj;                       //�`�[���L����2(�o�b�N�O���E���h)
    [SerializeField] GameObject teamChar3Obj;                       //�`�[���L����3(�o�b�N�O���E���h)
    [SerializeField] GameObject cursorObj;                          //�J�[�\���̃I�u�W�F�N�g

    [SerializeField] Image teamChar1IconObjImage;                   //�`�[���L����1�̃A�C�R����image
    [SerializeField] Image teamChar2IconObjImage;                   //�`�[���L����2�̃A�C�R����image
    [SerializeField] Image teamChar3IconObjImage;                   //�`�[���L����3�̃A�C�R����image

    [SerializeField] RectTransform cursorRect;                      //�J�[�\����RectTransform
    private int currentSelectTeamId = 0;                            //���ݑI�𒆂̃L����ID(�\�Ő���Ă���L�����̂��Ƃł͂Ȃ�)
    private int[] updateTeamId = new int[3];                        //�ύX��̗\��̃`�[��Id

    private int[] teamIdData = new int[3];                          //�`�[���f�[�^(ID)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�A�C�R���̃��t���b�V���B�`�[�������o�[��ς����Ƃ��A�������͊J�����u�Ԃ̂݌Ăяo��
    public void ChangeTeamMember() 
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        for(int i = 0; i < 3; i++) 
        {
            updateTeamId[i] = teamIdData[i];
            ChangeIcon(i, teamIdData[i]);
        }
    }

    //�`�[���Ґ��X�V
    public void UpdateTeam() 
    {
        teamCoutnrollerScript.ChangeTeam(updateTeamId);
    }

    //�`�[�������o�[���(�����_�ł͏d�������ꍇ�͑���𖳌���)
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
            //�L�����̌���
            updateTeamId[currentSelectTeamId] = charId;
            ChangeIcon(currentSelectTeamId, charId);
        }
        else
        {
            //�d���������̃L�����̌���
            (updateTeamId[conflictTeamId], updateTeamId[currentSelectTeamId]) = (updateTeamId[currentSelectTeamId], updateTeamId[conflictTeamId]);
            ChangeIcon(conflictTeamId, updateTeamId[conflictTeamId]);
            ChangeIcon(currentSelectTeamId, updateTeamId[currentSelectTeamId]);
        }
    }

    //�摜�̍����ւ�
    public void ChangeIcon(int teamCharId, int charId) 
    {
        if (teamCharId == 0) teamChar1IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        else if (teamCharId == 1) teamChar2IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
        else if (teamCharId == 2) teamChar3IconObjImage.sprite = changeTeamControllerObjScript.CharDbReferenceCharFullBodyImage(charId);
    }

    //�`�[�������o�[�ꗗ����I����
    public void SelectTeamMember(int teamId) 
    {
        currentSelectTeamId = teamId;

        //teamId�ɉ����Ĕz�u�̕ύX
        cursorRect.localPosition = new Vector2(teamId * 200 - 200, 0);
    }

    //�`�[�������o�[1�̃A�C�R�����N���b�N�����Ƃ�
    public void OnClickTeamMember1() => SelectTeamMember(0);

    //�`�[�������o�[2�̃A�C�R�����N���b�N�����Ƃ�
    public void OnClickTeamMember2() => SelectTeamMember(1);

    //�`�[�������o�[3�̃A�C�R�����N���b�N�����Ƃ�
    public void OnClickTeamMember3() => SelectTeamMember(2);

}

using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ChangeTeamController : MonoBehaviour
{
    [SerializeField] GameObject contentObj;         //content�I�u�W�F�N�g
    [SerializeField] GameObject charIconObj;        //charIcon(BackGround)�I�u�W�F�N�g
    [SerializeField] Image charFullBodyImage;       //charFullBodyImage�̃C���[�W
    [SerializeField] ChangeTeamTeamMemberController changeTeamTeamMemberControllerScript;   //ChangeTeamTeamMemberController�X�N���v�g

    [SerializeField] GameObject[] charIconObjs;     //

    private Sprite charIcon;                        //�L�����A�C�R��
    private Sprite charFullBodySprite;              //�L�����̗����G
    private int attribute;                          //�L�����̑���

    private int[] teamMemvers = new int[3];         //�`�[�������o�[�̕ϐ�(�����o�[�ϐ��ł������i�΁j)

    //�L�����N�^�[�f�[�^�x�[�X
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�Ґ��ύX��ʂ��J�����Ƃ�
    public void OpenChangeTeam() 
    {
        //�A�C�R���̃Z�b�g
        SetIcon();
        //TeamMember�̏�����
        changeTeamTeamMemberControllerScript.ChangeTeamMember();
        //�����G�̏�����
        charFullBodyImage.sprite = CharDbReferenceCharFullBodyImage(1);
    }

    //�Ґ��ύX��ʂ�����Ƃ�
    public void QuitChangeTeam() 
    {
        //�`�[���Ґ��X�V
        changeTeamTeamMemberControllerScript.UpdateTeam();
        //�A�C�R���̏���
        DestroyIcon();
    }

    //�����G�̍X�V(ChangeTeamIcon�̕����s���悤��)//�\�[�g�ɑΉ��ł���悤��
    public void ChangeCharFullBodyImage(int id) 
    {
        charFullBodyImage.sprite = CharDbReferenceCharFullBodyImage(id);
    }

    //�X�N���v�g�r���[�ɃA�C�R�����Z�b�g//�\�[�g�ɑΉ��ł���悤��
    public void SetIcon() 
    {
        //�X�N���v�g�r���[�ɃA�C�R�����Z�b�g
        foreach (var element in dB_charData.charData)
        {
            //0�̓f���L�����p�Ȃ̂ŏ��O
            if (element.charId != 0)
            {
                CharDbReference(element.charId);
                var charIconObjs = Instantiate(charIconObj, this.transform.position, this.transform.rotation);
                charIconObjs.transform.parent = contentObj.transform;
                charIconObjs.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                GameObject childCharIconObj = charIconObjs.transform.Find("CharIcon").gameObject;
                Image childCharIconImage = childCharIconObj.GetComponent<Image>();
                childCharIconImage.sprite = charIcon;
                ChangeTeamIcon charIconObjsScript = charIconObjs.GetComponent<ChangeTeamIcon>();
                charIconObjsScript.SetData(element.charId);
            }
        }
    }

    //�A�C�R���̏���
    public void DestroyIcon() 
    {
        foreach (Transform child in contentObj.transform) 
        {
            if (child.name == "CharIconBackGround(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }

    //�L���������f�[�^�x�[�X����Q��
    public void CharDbReference(int charId)
    {
        charIcon = dB_charData.charData[charId].charIcon;                   //�L�����A�C�R��
        charFullBodySprite = dB_charData.charData[charId].charFullBodyImage; //�L���������G
        attribute = dB_charData.charData[charId].attribute;                 //����
    }

    //�L���������f�[�^�x�[�X����Q��
    public Sprite CharDbReferenceCharFullBodyImage(int charId)
    {
        charFullBodySprite = dB_charData.charData[charId].charFullBodyImage; //�L���������G
        return charFullBodySprite;
    }

    //�L���������f�[�^�x�[�X����Q��
    public Sprite CharDbReferenceCharIcon(int charId)
    {
        Sprite charIconSprite = dB_charData.charData[charId].charIcon;      //�L�����A�C�R��
        return charIconSprite;
    }
}

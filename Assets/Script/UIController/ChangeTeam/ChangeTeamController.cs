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

    private Sprite charIcon;                        //�L�����A�C�R��
    private Sprite charFullBodyImage;               //�L�����̗����G
    private int attribute;                          //�L�����̑���

    private int[] teamMemvers = new int[3];         //�`�[�������o�[�̕ϐ�(�����o�[�ϐ��ł������i�΁j)

    //�L�����N�^�[�f�[�^�x�[�X
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    //�L���������f�[�^�x�[�X����Q��
    public void CharDbReference(int charId)
    {
        charIcon = dB_charData.charData[charId].charIcon;                   //�L�����A�C�R��
        charFullBodyImage = dB_charData.charData[charId].charFullBodyImage; //�L���������G
        attribute = dB_charData.charData[charId].attribute;                 //����
    }

    //�L���������f�[�^�x�[�X����Q��
    public Sprite CharDbReferenceCharFullBodyImage(int charId)
    {
        charFullBodyImage = dB_charData.charData[charId].charFullBodyImage; //�L���������G
        return charFullBodyImage;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    private string thisObjName;                         //���g�̖��O

    private int currentCharId;                          //���݂̕\�̃L����ID(�ύX�O)
    private int[] currentTeamCharId = new int[3];       //���݂̃`�[���L������Id

    [SerializeField] Sprite charIcon;                   //�L�����A�C�R��
    private Image thisImage;                            //���g��Image

    [SerializeField] TeamCoutnroller teamController;    //�`�[���R���g���[���X�N���v�g
    public DB_CharData dB_charData;                     //�L�����N�^�[�f�[�^�x�[�X

    [SerializeField] Color32 deathIconColor = new Color32(125, 125, 125, 175);   //���S���̃L�����A�C�R���̃O���[�X�P�[����

    // Start is called before the first frame update
    void Start()
    {
        thisImage = gameObject.GetComponent<Image>();

        //���g�̃I�u�W�F�N�g�����擾
        thisObjName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisObjName == "PlayerIcon")
        {
            if (currentCharId != teamController.CharId)
            {
                CharChange();
            }
        }
        else if (thisObjName == "Sub1Icon") 
        {
            if (currentTeamCharId[0] != teamController.TeamIdData[0]) 
            {
                TeamChange(0);
            }
            DeathCharIcon(0);
        }
        else if (thisObjName == "Sub2Icon")
        {
            if (currentTeamCharId[1] != teamController.TeamIdData[1])
            {
                TeamChange(1);
            }
            DeathCharIcon(1);
        }
        else if (thisObjName == "Sub3Icon")
        {
            if (currentTeamCharId[2] != teamController.TeamIdData[2])
            {
                TeamChange(2);
            }
            DeathCharIcon(2);
        }
    }

    //�L�����`�F���W���o��
    public void CharChange()
    {
        CharDbReference(teamController.CharId);                 //���ݎg�p���̃L�����̎Q��
        if (charIcon != null)
        {
            thisImage.sprite = charIcon;
        }
        else 
        {
            thisImage.sprite = null;
        }
        currentCharId = teamController.CharId;
    }

    //�`�[���`�F���W���o��(�\�̃L����)
    public void TeamChange(int teamId)
    {
        CharDbReference(teamController.TeamIdData[teamId]);
        if (charIcon != null)
        {
            thisImage.sprite = charIcon;
        }
        else
        {
            thisImage.sprite = null;
        }
    }

    //���S�L�����̃A�C�R�����O���[�X�P�[����
    public void DeathCharIcon(int teamId) 
    {
        if (teamController.TeamCurrentHp[teamId] <= 0 && thisImage.color != deathIconColor) 
        {
            thisImage.color = deathIconColor;
        }
        if (teamController.TeamCurrentHp[teamId] > 0 && thisImage.color == deathIconColor)  
        {
            thisImage.color = new Color32(255, 255, 255, 255);
        }
    }
    //�L�������Q��
    public void CharDbReference(int updateCharId) 
    {
        charIcon = dB_charData.charData[updateCharId].charIcon;                  //�L�����A�C�R��
    }
}

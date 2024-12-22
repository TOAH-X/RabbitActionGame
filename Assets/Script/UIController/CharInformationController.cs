using System.Buffers.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharInformationController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI charNameText;                  //�L�������e�L�X�g
    [SerializeField] TextMeshProUGUI charLevelText;                 //�L�������x���e�L�X�g
    [SerializeField] TextMeshProUGUI charAttributeText;             //�����e�L�X�g
    [SerializeField] TextMeshProUGUI charWeaponTypeText;            //�����e�L�X�g
    [SerializeField] TextMeshProUGUI charAttentionRateText;         //��S���\�L�e�L�X�g
    [SerializeField] TextMeshProUGUI charAttentionDamageText;       //��S�_���[�W�\�L�e�L�X�g
    [SerializeField] TextMeshProUGUI charHpText;                    //HP�\�L�e�L�X�g
    [SerializeField] TextMeshProUGUI charAttackText;                //�U���͕\�L�e�L�X�g
    [SerializeField] TextMeshProUGUI characteristicNameText;        //�������e�L�X�g
    [SerializeField] TextMeshProUGUI characteristicExplanationText; //���������e�L�X�g
    [SerializeField] TextMeshProUGUI skillNameText;                 //�X�L�����e�L�X�g
    [SerializeField] TextMeshProUGUI skillExplanationText;          //�X�L�������e�L�X�g
    [SerializeField] TextMeshProUGUI specialMoveNameText;           //�K�E�Z���e�L�X�g
    [SerializeField] TextMeshProUGUI specialMoveExplanationText;    //�K�E�Z�����e�L�X�g

    [SerializeField] Image charFullBodyImage;                       //�����G

    [SerializeField] GameObject charInformationBackGround;          //�L�������̔w�i

    [SerializeField] GameObject playerObj;                          //�v���C���[�I�u�W�F�N�g
    [SerializeField] Player playerScript;                           //�v���C���[�X�N���v�g

    private int charId = 0;                                         //�Q�Ƃ���L����ID

    private string charName;                                        //�L������
    private int attribute;                                          //����
    private int weaponType;                                         //�����
    private string characteristicName;                              //������
    private string characteristicExplanation;                       //��������
    private string skillName;                                       //�X�L����
    private string skillExplanation;                                //�X�L������
    private string specialMoveName;                                 //�K�E�Z��
    private string specialMoveExplanation;                          //�K�E�Z����

    public DB_CharData dB_charData;                                 //�L�����N�^�[�f�[�^�x�[�X

    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerObj.GetComponent<Player>();
        charId = playerScript.CharId;
        CharDbReference();
        charInformationText();
    }

    // Update is called once per frame
    void Update()
    {
        if (charInformationBackGround.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                charInformationBackGround.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                charInformationBackGround.SetActive(false);
            }
        }

        charId = playerScript.CharId;
        CharDbReference();
        charInformationText();
    }

    //�L���������f�[�^�x�[�X����Q��
    public void CharDbReference()
    {
        charName = dB_charData.charData[charId].charName;                                   //�L������
        attribute = dB_charData.charData[charId].attribute;                                 //����
        weaponType = dB_charData.charData[charId].weaponType;                               //�����
        characteristicName = dB_charData.charData[charId].characteristicName;               //������
        characteristicExplanation = dB_charData.charData[charId].characteristicExplanation; //��������
        skillName = dB_charData.charData[charId].skillName;                                 //�X�L����
        skillExplanation = dB_charData.charData[charId].skillExplanation;                   //�X�L������
        specialMoveName = dB_charData.charData[charId].specialMoveName;                     //�K�E�Z��
        specialMoveExplanation = dB_charData.charData[charId].specialMoveExplanation;       //�K�E�Z����
        charFullBodyImage.sprite = dB_charData.charData[charId].charFullBodyImage;          //�����G
    }

    //�L���������e�L�X�g�ɔ��f
    public void charInformationText() 
    {
        string attributeName = "----";
        if (attribute == 0)
        {
            attributeName = "����(����ȑ����͂Ȃ�)";
        }
        else if (attribute == 1) 
        {
            attributeName = "��";
        }
        else if (attribute == 2)
        {
            attributeName = "��";
        }
        else if (attribute == 3)
        {
            attributeName = "��";
        }
        else if (attribute == 4)
        {
            attributeName = "�y";
        }
        else if (attribute == 5)
        {
            attributeName = "�G�[�e��";
        }
        else if (attribute == 6)
        {
            attributeName = "����";
        }
        else
        {
            attributeName = "����ȑ����͂Ȃ�";
        }
        charNameText.text = charName;                                                           //�L������
        charLevelText.text = "Lv." + 99;                                                        //�L�������x���e�L�X�g
        charAttributeText.text = "����:" + attributeName;                                       //�����e�L�X�g
        charWeaponTypeText.text = "�����:----";                                                  //�����e�L�X�g
        charAttentionRateText.text = "��S��:" + playerScript.AttentionRate.ToString("F1") + "%";              //��S���\�L�e�L�X�g
        charAttentionDamageText.text = "��S�_���[�W:" + playerScript.AttentionDamage.ToString("F1") + "%";    //��S�_���[�W�\�L�e�L�X�g
        charHpText.text = "�ő�HP:" + playerScript.MaxHp;                                           //HP�\�L�e�L�X�g
        charAttackText.text = "�U����:" + playerScript.Attack;                                  //�U���͕\�L�e�L�X�g
        characteristicNameText.text = "�����u" + characteristicName + "�v";                     //�������e�L�X�g
        characteristicExplanationText.text = characteristicExplanation;                         //���������e�L�X�g
        skillNameText.text = "�X�L���u" + skillName + "�v";                                     //�X�L�����e�L�X�g
        skillExplanationText.text = skillExplanation;                                           //�X�L�������e�L�X�g
        specialMoveNameText.text = "�K�E�Z�u" + specialMoveName + "�v";                         //�K�E�Z���e�L�X�g
        specialMoveExplanationText.text = specialMoveExplanation;                               //�K�E�Z�����e�L�X�g
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GaugeCountroller : MonoBehaviour
{
    [SerializeField] float gaugeLength;                              //�Q�[�W�̒���(��{�I�ɐ��l��1)
    [SerializeField] int maxValue;                                  //�l�̍ő�l(�ő�HP�Ȃ�)
    [SerializeField] int currentValue;                              //���݂̒l(����HP�Ȃ�)

    [SerializeField] Transform canvasTransform;                     //Canvas��������(�_���[�W�\���p�Ȃ�)
    [SerializeField] GameObject gaugeValueNotationObj;              //�Q�[�W�\�L�I�u�W�F�N�g
    [SerializeField] GameObject gaugeValueNotationObjs;             //�v���n�u�̃Q�[�W�\�L�I�u�W�F�N�g
    [SerializeField] GaugeValueNotationCountroller gaugeValueNotationObjsScript;    //�Q�[�W�̕\�L�I�u�W�F�N�g�̃X�N���v�g

    [SerializeField] string thisObjName;                            //�A�^�b�`���ꂽ�I�u�W�F�N�g�̖��O(�Q�[�W�̎�ނ��Ǘ����邽��)

    [SerializeField] Player player;                                 //getset�p(Player�X�e�[�^�X)
    [SerializeField] EnemyHP enemyHp;                               //getset�p(EnemyHP)
    [SerializeField] TeamCoutnroller teamCoutnroller;               //getset�p(�`�[���L�����̃X�e�[�^�X)

    private RectTransform rectTransform;                            //Image�̏��擾

    private GameObject staminaParentObj;                            //�X�^�~�i�Q�[�W�̐e
    private Vector3 staminaParentObjRotation;                       //�X�^�~�i�Q�[�W�̐e�̉�]���

    private Color32[] hPGaugeColor = new Color32[4];                //HP�Q�[�W�̐F

    private Image thisImage;                                        //���g��Image

    // Start is called before the first frame update
    void Start()
    {
        /*
        this.player = FindObjectOfType<Player>(); // �C���X�^���X��
        int currentHp = player.CurrentHp; // �Q�b�^�[�BScriptA�̕ϐ����擾����
        */
        //getset�p�ɃC���X�^���X��
        this.player = FindObjectOfType<Player>();
        this.enemyHp = FindObjectOfType<EnemyHP>();


        //���g�̃I�u�W�F�N�g�����擾
        thisObjName = gameObject.name;

        //�����l(��)
        maxValue = 100;
        currentValue = 100;

        //�Q�[�W�̒����̎擾
        //gageLength = this.transform.localScale.x;
        rectTransform = GetComponent<RectTransform>();
        gaugeLength = rectTransform.localScale.x;

        //HP�Q�[�W�̂Ƃ�����̏���(���l�̕\���Ȃ�)
        if (thisObjName == "HPGaugeMain")
        {
            //Canvas��������(HP�l�\���p�Ȃ�)
            canvasTransform = GameObject.Find("Canvas").transform;

            //HP(�Q�[�W�̐��l)�\�L�Ăяo��(�v���n�u)
            gaugeValueNotationObjs = Instantiate<GameObject>(gaugeValueNotationObj, transform.position, Quaternion.identity, canvasTransform);
            gaugeValueNotationObjsScript = gaugeValueNotationObjs.GetComponent<GaugeValueNotationCountroller>();
        }
        //HP�Q�[�W�̏ꍇ(�y�ʉ�)
        if (thisObjName == "HPGaugeMain" || thisObjName == "Sub1HPGaugeMain" || thisObjName == "Sub2HPGaugeMain" || thisObjName == "Sub3HPGaugeMain") 
        {
            thisImage = gameObject.GetComponent<Image>();
            hPGaugeColor[0] = new Color32(20, 200, 50, 255);
            hPGaugeColor[1] = new Color32(200, 200, 20, 255);
            hPGaugeColor[2] = new Color32(200, 150, 20, 255);
            hPGaugeColor[3] = new Color32(200, 50, 20, 255);
        }
        //�X�^�~�i�Q�[�W
        else if (thisObjName == "StaminaGaugeMain")
        {
            //�e�̎擾
            staminaParentObj = transform.parent.gameObject;
            staminaParentObjRotation = staminaParentObj.transform.localEulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Human human = new Human();
        //human.Height = 150; // Height�Őݒ�
        //Debug.Log(human.Height); // Height�Ŏ擾

        //�l�̎擾
        if (thisObjName == "HPGaugeMain")
        {
            //HP�l�̎󂯓n��
            maxValue = player.MaxHp;
            currentValue = player.CurrentHp;
        }
        else if (thisObjName == "MPGaugeMain")
        {
            //MP�l�̎󂯓n��
            maxValue = player.MaxMp;
            currentValue = player.CurrentMp;
        }
        else if (thisObjName == "StaminaGaugeMain")
        {
            //Stamina�l�̎󂯓n��
            maxValue = player.MaxStamina;
            currentValue = player.CurrentStamina;
            //�X�^�~�i���ő�̎��͉�]���ĉB��
            if (maxValue <= currentValue) 
            {
                staminaParentObj.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else 
            {
                staminaParentObj.transform.localEulerAngles = staminaParentObjRotation;
            }
        }
        else if (thisObjName == "EnemyHPGaugeMain") 
        {
            //EnemyHP�l�̎󂯓n��
            maxValue = enemyHp.EnemyMaxHp;
            currentValue = enemyHp.EnemyCurrentHp;
        }
        else if (thisObjName == "Sub1HPGaugeMain")
        {
            //Sub1HP�l�̎󂯓n��
            maxValue = teamCoutnroller.TeamMaxHp[0];
            currentValue = teamCoutnroller.TeamCurrentHp[0];
            //HP�Q�[�W�̐F�̕ύX
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
        else if (thisObjName == "Sub2HPGaugeMain")
        {
            //Sub2HP�l�̎󂯓n��
            maxValue = teamCoutnroller.TeamMaxHp[1];
            currentValue = teamCoutnroller.TeamCurrentHp[1];
            //HP�Q�[�W�̐F�̕ύX
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
        else if (thisObjName == "Sub3HPGaugeMain")
        {
            //Sub3HP�l�̎󂯓n��
            maxValue = teamCoutnroller.TeamMaxHp[2];
            currentValue = teamCoutnroller.TeamCurrentHp[2];
            //HP�Q�[�W�̐F�̕ύX
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }

        //���ݒn���ő�l�𒴂����ۂɂ͂ݏo���Ȃ��悤�ɂ��鏈��
        if ((float)((float)currentValue / (float)maxValue) > 1) 
        {
            currentValue = maxValue;
        }

        if (maxValue != 0) 
        {
            //�Q�[�W���̔��f
            rectTransform.localScale = new Vector3(gaugeLength * currentValue / maxValue, 1, 1);
            rectTransform.localPosition = new Vector3(((gaugeLength * currentValue / maxValue) * 0.5f - 0.5f) * rectTransform.sizeDelta.x, 0, 0);
            //this.transform.localScale = new Vector3(gageLength * currentValue / maxValue, 1, 1);
            //this.transform.localPosition = new Vector3((gageLength * currentValue / maxValue) * 0.5f -0.5f, 0, 0);
        }

        //HP(�Q�[�W�̒l)�̍X�V
        if (thisObjName == "HPGaugeMain") 
        {
            //�l�\�L�̔��f
            gaugeValueNotationObjsScript.GaugeValueNotation(maxValue, currentValue);
            //HP�Q�[�W�̐F�̕ύX
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
    }

    //HP�Q�[�W�̃J���[�̕ύX(����HP�̊����ɉ�����)
    public void ChangeHPGaugeColor(float valueRate) 
    {
        if (valueRate >= 0.75f)
        {
            thisImage.color = hPGaugeColor[0];
        }
        else if (valueRate >= 0.50f) 
        {
            thisImage.color = hPGaugeColor[1];
        }
        else if (valueRate >= 0.25f)
        {
            thisImage.color = hPGaugeColor[2];
        }
        else
        {
            thisImage.color = hPGaugeColor[3];
        }
    }
}

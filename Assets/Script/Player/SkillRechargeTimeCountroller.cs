using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillRechargeTimeCountroller : MonoBehaviour
{
    [SerializeField] Player playerScript;                           //�v���C���[�X�N���v�g
    private float currentSkillRecharge = 0;                         //�X�L���N�[���^�C���̎c�莞��
    [SerializeField] Text thisText;                                 //�e�L�X�g
    private string thisObjName;                                     //�I�u�W�F�N�g�̖��O(�K�E�Z���X�L������ʂ��邽��)

    // Start is called before the first frame update
    void Start()
    {
        thisText = this.GetComponent<Text>();

        //�K�E�Z�ƃX�L���̃N�[���^�C���𕹗p����
        thisObjName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {            
        //�N�[���^�C���̎擾
        if (thisObjName == "SkillRechargeTime")
        {
            currentSkillRecharge = playerScript.CurrentSkillRecharge;
        }
        else if (thisObjName == "SpecialMoveRechargeTime") 
        {
            currentSkillRecharge = playerScript.CurrentSpecialMoveRecharge;
        }

        if (currentSkillRecharge != 0)
        {
            //�N�[���^�C���̕\��
            thisText.text = "" + currentSkillRecharge.ToString("n2");
        }
        else 
        {
            if (thisObjName == "SkillRechargeTime")
            {
                //��\��
                thisText.text = "E";
            }
            else if (thisObjName == "SpecialMoveRechargeTime")
            {
                //��\��
                thisText.text = "Q";
            }
                
        }
            
    }
}

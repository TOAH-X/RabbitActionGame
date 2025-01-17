using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharData
{
    [Header("�L����ID")]
    public int charId;                                  //�L�����N�^�[��ID
    [Header("�L������")]
    public string charName;                             //�L�����N�^�[��
    [Header("���A�x")]
    public string rarity;                               //���A�x
    [Header("����")]
    public int attribute;                               //����(0:������(����)�A1:�΁A2:���A3:���A4:�y�A5:�G�[�e���A6:����(�P�m��))
    [Header("�����")]
    public int weaponType = 0;                          //�����
    //public byte charLevel;                            //�L�������x��
    //public int charExp;                               //�L�����o���l
    //public int maxStamina;                            //�ő�X�^�~�i
    [Header("��bHP")]
    public int baseHp;                                  //��bHP
    //public int maxHp;                                 //�ő�HP
    [Header("HP�o�t")]
    public float buffHp;                                //HP�o�t
    //public int currentHp;                             //����HP
    //public int maxMp;                                 //�ő�MP
    //public int currentMp;                             //����MP
    [Header("��b�U����")]
    public int baseAttack;                              //��b�U����
    //public int attack;                                //�U����
    //public float buffAttack;                          //�U���̓o�t
    [Header("��b��S�_���[�W")]
    public float baseAttentionDamage = 150;             //��b��S�_���[�W(�U��+��S�_���[�W(��))��)��S�_���[�W150%�͒ʏ��2.5�{
    [Header("��b��S��")]
    public float baseAttentionRate = 30;                //��b��S��(�����100%)

    [Header("������")]
    public string characteristicName;                   //������
    [Header("��������"), TextArea]
    public string characteristicExplanation;            //��������
    [Header("�X�L����")]
    public string skillName;                            //�X�L����
    [Header("�X�L������"), TextArea]
    public string skillExplanation;                     //�X�L������
    [Header("�K�E�Z��")]
    public string specialMoveName;                      //�K�E�Z��
    [Header("�K�E�Z����"), TextArea]
    public string specialMoveExplanation;               //�K�E�Z����

    [Header("�X�L���N�[���^�C��")]
    public float maxSkillRecharge;                      //�X�L���̃N�[���^�C��(�b)
    [Header("�K�E�Z�R�X�g")]
    public int maxSpecialMoveEnergy;                    //�K�E�Z�̃R�X�g
    [Header("�K�E�Z�N�[���^�C��")]
    public int maxSpecialMoveRecharge;                  //�K�E�Z�̃N�[���^�C��(�b)

    //public int weaponId = 0;                          //�������Ă��镐���ID

    //public int sousinguId = 0;                        //�������Ă��鑕�g��(���╨)��ID

    [Header("��b�ړ����x")]
    public float baseMoveSpeed = 3.0f;                  //��b�ړ����x
    //public float buffMoveSpeed = 3.0f;                //�ړ����x�o�t
    //public float jumpForce = 12.0f;                   //�W�����v��
    //public float dashForce = 5.0f;                    //�_�b�V����

    [Header("�A�C�R���摜")]
    public Sprite charIcon;                              //�A�C�R���摜
    [Header("�����G")]
    public Sprite charFullBodyImage;                     //�����G
    //[Header("�Q�[�����L����")]
    //public Sprite charAnimation;                              //�Q�[�����L����
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //�G��ID
    [SerializeField] int enemyAttribute = 0;                    //�G�̑����ADB�Q��
    [SerializeField] float enemyMaxHpBaseRate;                  //�G�̊�bHP�{��
    [SerializeField] float enemyAttackBaseRate;                 //�G�̊�b�U���͔{��

    [SerializeField] int enemyLevel = 99;                       //�G�̃��x��

    [SerializeField] int enemyMaxHp;                            //�G��HP
    [SerializeField] int enemyAttack;                           //�G�̍U����

    [SerializeField] Sprite enemyPicture;                       //�G�̉摜

    //�L�����N�^�[�f�[�^�x�[�X
    public DB_EnemyData dB_enemyData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�X�|�i�[����G�̃f�[�^���󂯎��
    public void EnemyData(int iD, int level)
    {
        enemyId = iD;
        enemyLevel = level;

        //�G��DB�̎Q��
        EnemyDbReference();

        //�ő�HP�̌v�Z
        enemyMaxHp = Mathf.RoundToInt(enemyMaxHpBaseRate * (10.0f * Mathf.Pow(enemyLevel, 2) + 100.0f * enemyLevel + 100));
        //�U���͂̌v�Z
        enemyAttack = Mathf.RoundToInt(enemyMaxHpBaseRate * (0.01f * Mathf.Pow(enemyLevel, 2) + 10.0f * enemyLevel + 10));
    }

    //�G�����f�[�^�x�[�X����Q��
    public void EnemyDbReference()
    {
        enemyMaxHpBaseRate = dB_enemyData.enemyData[enemyId].enemyMaxHpBaseRate;    //�G�̊�bHP�{��
        enemyAttackBaseRate = dB_enemyData.enemyData[enemyId].enemyAttackBaseRate;  //�G�̊�b�U���͔{��
        enemyAttribute = dB_enemyData.enemyData[enemyId].enemyAttribute;            //�G�̑���
        enemyPicture = dB_enemyData.enemyData[enemyId].enemyPicture;                //�G�̉摜
    }

    //�GID�󂯓n��
    public int EnemyId => enemyId;
    //�G�����󂯓n��
    public int EnemyAttribute => enemyAttribute;
    //�G�̍ő�HP�󂯓n��
    public int EnemyMaxHp => enemyMaxHp;
    //�G�̍U���͎󂯓n��
    public int EnemyAttack => enemyAttack;

    //�G�̍U���͎󂯓n��
    public Sprite EnemyPicture => enemyPicture;


    //�G���x���󂯓n��
    public int EnemyLevel => enemyLevel;

}

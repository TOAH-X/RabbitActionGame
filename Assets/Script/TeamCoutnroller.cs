using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamCoutnroller : MonoBehaviour
{
    [SerializeField] Player playerScript;                       //�v���C���[�X�N���v�g
    [SerializeField] int currentChar;                           //���ݕ\�̃L����(�`�[���̉��Ԗڂ̃L������)
    [SerializeField] int charId;                                //�L����ID
    [SerializeField] int[] teamIdData = new int[3];             //�`�[���Ґ��̃L����ID�f�[�^�����i�[
    [SerializeField] bool[] isTeamCharAlive = new bool[3];      //�L�����̐�������(true�����Afalse����)
    [SerializeField] GameObject sub1Obj;                        //�T�u1�̃A�C�R����HP�Q�[�W��Z�߂�I�u�W�F�N�g
    [SerializeField] GameObject sub2Obj;                        //�T�u2�̃A�C�R����HP�Q�[�W��Z�߂�I�u�W�F�N�g
    [SerializeField] GameObject sub3Obj;                        //�T�u3�̃A�C�R����HP�Q�[�W��Z�߂�I�u�W�F�N�g

    [SerializeField] float teamChangeTimer = 0;                 //�L�����`�F���W�̃N�[���^�C���̎c�莞��
    [SerializeField] float teamChangeCoolTime = 0.1f;           //�L�����`�F���W�̃N�[���^�C��

    [SerializeField] int[] teamMaxHpData = new int[3];                          //�`�[���̍ő�HP�f�[�^
    [SerializeField] int[] teamCurrentHpData = new int[3];                      //�`�[���̌���HP�f�[�^
    [SerializeField] float[] teamCurrentSkillRechargeData = new float[3];       //�`�[���̌��݃X�L���N�[���^�C���f�[�^
    [SerializeField] float[] teamCurrentSpecialMoveRechargeData = new float[3]; //�`�[���̌��ݕK�E�Z�N�[���^�C���f�[�^

    // Start is called before the first frame update
    void Start()
    {
        playerScript = this.gameObject.GetComponent<Player>();

        //�z��̏�����(�G���[�΍�)
        if (teamIdData == null || teamIdData.Length < 3) 
        {
            teamIdData = new int[3];
        }
        //�L�����f�[�^�����ɑ��
        for (int i = 0; i < 3; i++) 
        {
            teamIdData[i] = i + 1;
        }

        //�z��̏�����(�G���[�΍�)
        if (teamCurrentHpData == null || teamCurrentHpData.Length < 3)
        {
            teamCurrentHpData = new int[3];
        }
        //�L�����f�[�^�����ɑ��
        for (int i = 0; i < 3; i++)
        {
            teamCurrentHpData[i] = 1;
            isTeamCharAlive[i] = true;
        }

        teamCurrentHpData[0] = playerScript.CurrentHp;
        teamCurrentHpData[1] = playerScript.CurrentHp;
        teamCurrentHpData[2] = playerScript.CurrentHp;

        //��l�ڂ������ꍇ
        currentChar = 0;
        sub1Obj.transform.localPosition = new Vector3(25f, 0, 0);
        sub2Obj.transform.localPosition = new Vector3(0, 0, 0);
        sub3Obj.transform.localPosition = new Vector3(0, 0, 0);

        GetPlayerData(currentChar);
        SetPlayerData(0);

        playerScript.CharChange(teamIdData[0]);
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerData(currentChar);

        //�L�������S���A�����t���O�ύX�Ƌ����`�F���W
        if (teamCurrentHpData[0] <= 0 && isTeamCharAlive[0] == true)  
        {
            isTeamCharAlive[0] = false;
            if (teamCurrentHpData[1] <= 0)
            {
                CharChange(2);
            }
            CharChange(1);
        }
        if (teamCurrentHpData[1] <= 0 && isTeamCharAlive[1] == true)
        {
            isTeamCharAlive[1] = false;
            if (teamCurrentHpData[1] <= 0)
            {
                CharChange(2);
            }
            CharChange(0);
        }
        if (teamCurrentHpData[2] <= 0 && isTeamCharAlive[2] == true)
        {
            isTeamCharAlive[2] = false;
            if (teamCurrentHpData[1] <= 0)
            {
                CharChange(1);
            }
            CharChange(0);
        }
        //�L�����`�F���W
        if (teamChangeTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && teamCurrentHpData[0] > 0)
            {
                CharChange(0);
                teamChangeTimer = teamChangeCoolTime;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && teamCurrentHpData[1] > 0)
            {
                CharChange(1);
                teamChangeTimer = teamChangeCoolTime;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && teamCurrentHpData[2] > 0)
            {
                CharChange(2);
                teamChangeTimer = teamChangeCoolTime;
            }
            //�N�[���^�C�����}�C�i�X�ɂȂ����ꍇ
            if (teamChangeTimer < 0)
            {
                teamChangeTimer = 0;
            }
        }
        else
        {
            teamChangeTimer -= Time.deltaTime;
        }

        //�Q�[���I�[�o�[
        if(teamCurrentHpData[0] <= 0&& teamCurrentHpData[1] <= 0&& teamCurrentHpData[2] <= 0) 
        {
            Debug.Log("�Q�[���I�[�o�[");
            //�����[�h
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    //�L�����ύX
    public void CharChange(int updateCharId)
    {
        //3�L�����ɓK�������ꍇ
        if (teamCurrentHpData[updateCharId] > 0)
        {
            Debug.Log(updateCharId + "�l�ڂɕύX");
            sub1Obj.transform.localPosition = new Vector3(0, 0, 0);
            sub2Obj.transform.localPosition = new Vector3(0, 0, 0);
            sub3Obj.transform.localPosition = new Vector3(0, 0, 0);
            if (updateCharId == 0) 
            {
                sub1Obj.transform.localPosition = new Vector3(25f, 0, 0);
            }
            if (updateCharId == 1)
            {
                sub2Obj.transform.localPosition = new Vector3(25f, 0, 0);
            }
            if (updateCharId == 2)
            {
                sub3Obj.transform.localPosition = new Vector3(25f, 0, 0);
            }
            
            SetPlayerData(updateCharId);

            playerScript.CharChange(teamIdData[updateCharId]);

            currentChar = updateCharId;
        }
    }

    //�v���C���[����f�[�^���擾
    public void GetPlayerData(int currentCharId) 
    {
        teamMaxHpData[currentCharId] = playerScript.MaxHp;                                                //�ő�HP
        teamCurrentHpData[currentCharId] = playerScript.CurrentHp;                                        //����HP
        teamCurrentSkillRechargeData[currentCharId] = playerScript.CurrentSkillRecharge;                  //�X�L���N�[���^�C��
        teamCurrentSpecialMoveRechargeData[currentCharId] = playerScript.CurrentSpecialMoveRecharge;      //�X�L���N�[���^�C��
    }

    //�v���C���[�ɑ΂��ăf�[�^����������
    public void SetPlayerData(int updatedCharId)
    {
        playerScript.MaxHp = teamMaxHpData[updatedCharId];                                                  //�ő�HP
        playerScript.CurrentHp = teamCurrentHpData[updatedCharId];                                          //����HP
        playerScript.CurrentSkillRecharge = teamCurrentSkillRechargeData[updatedCharId];                    //�X�L���N�[���^�C��
        playerScript.CurrentSpecialMoveRecharge = teamCurrentSpecialMoveRechargeData[updatedCharId];        //�X�L���N�[���^�C��
    }

    //maxHp�Q�Ɨp(getset)

    public int[] TeamMaxHp // �v���p�e�B
    {
        get { return teamMaxHpData; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { teamMaxHpData = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //currentHp�Q�Ɨp(getset)

    public int[] TeamCurrentHp // �v���p�e�B
    {
        get { return teamCurrentHpData; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { teamCurrentHpData = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
}
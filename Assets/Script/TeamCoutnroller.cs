using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamCoutnroller : MonoBehaviour
{
    [SerializeField] Player playerScript;                       //プレイヤースクリプト
    [SerializeField] int currentChar;                           //現在表のキャラ(チームの何番目のキャラか)
    [SerializeField] int charId;                                //キャラID
    [SerializeField] int[] teamIdData = new int[3];             //チーム編成のキャラIDデータ情報を格納
    [SerializeField] bool[] isTeamCharAlive = new bool[3];      //キャラの生存判定(trueが生、falseが死)
    [SerializeField] GameObject sub1Obj;                        //サブ1のアイコンとHPゲージを纏めるオブジェクト
    [SerializeField] GameObject sub2Obj;                        //サブ2のアイコンとHPゲージを纏めるオブジェクト
    [SerializeField] GameObject sub3Obj;                        //サブ3のアイコンとHPゲージを纏めるオブジェクト

    [SerializeField] float teamChangeTimer = 0;                 //キャラチェンジのクールタイムの残り時間
    [SerializeField] float teamChangeCoolTime = 0.1f;           //キャラチェンジのクールタイム

    [SerializeField] int[] teamMaxHpData = new int[3];                          //チームの最大HPデータ
    [SerializeField] int[] teamCurrentHpData = new int[3];                      //チームの現在HPデータ
    [SerializeField] float[] teamCurrentSkillRechargeData = new float[3];       //チームの現在スキルクールタイムデータ
    [SerializeField] float[] teamCurrentSpecialMoveRechargeData = new float[3]; //チームの現在必殺技クールタイムデータ

    //キャラクターデータベース
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = this.gameObject.GetComponent<Player>();

        //配列の初期化(エラー対策)
        if (teamIdData == null || teamIdData.Length < 3) 
        {
            teamIdData = new int[3];
        }
        //チームデータを仮に代入
        for (int i = 0; i < 3; i++) 
        {
            teamIdData[i] = i + 1;
        }
        teamIdData[0] = 6;
        teamIdData[1] = 2;
        teamIdData[2] = 4;

        //配列の初期化(エラー対策)
        if (teamCurrentHpData == null || teamCurrentHpData.Length < 3)
        {
            teamCurrentHpData = new int[3];
        }
        //キャラデータを仮に代入
        for (int i = 0; i < 3; i++)
        {
            teamCurrentHpData[i] = 1;
            isTeamCharAlive[i] = true;
        }

        //最終的には保管している情報から現在HPを代入すること(チーム更新時のための最大HPを読み込む関数を作ること)
        for(int i = 0; i < 3; i++)
        {
            teamMaxHpData[i] = dB_charData.charData[teamIdData[i]].baseHp;              //最大HP
            teamCurrentHpData[i] = dB_charData.charData[teamIdData[i]].baseHp;          //現在HP
        }

        //一人目だった場合
        currentChar = 0;
        sub1Obj.transform.localPosition = new Vector3(25f, 0, 0);
        sub2Obj.transform.localPosition = new Vector3(0, 0, 0);
        sub3Obj.transform.localPosition = new Vector3(0, 0, 0);

        GetPlayerData(currentChar);
        SetPlayerData(0);

        playerScript.CharChange(teamIdData[0]);                                 //プレイヤースクリプトにキャラ変更した情報を渡す

        charId = teamIdData[currentChar];                                       //現在の表のキャラのキャラIDの更新
    }

    // Update is called once per frame
    void Update()
    {
        //チーム変更
        if (Input.GetKey(KeyCode.Z)) 
        {
            //ChangeTeam();
        }

        GetPlayerData(currentChar);

        //キャラ死亡時、生存フラグ変更と強制チェンジ
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
        //キャラチェンジ
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
            //クールタイムがマイナスになった場合
            if (teamChangeTimer < 0)
            {
                teamChangeTimer = 0;
            }
        }
        else
        {
            teamChangeTimer -= Time.deltaTime;
        }

        //ゲームオーバー
        if(teamCurrentHpData[0] <= 0&& teamCurrentHpData[1] <= 0&& teamCurrentHpData[2] <= 0) 
        {
            Debug.Log("ゲームオーバー");
            //リロード
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //クールタイム関係
        TeamCharRecharge();
    }

    //キャラ変更
    public void CharChange(int updateCharId)
    {
        //3キャラに適合した場合
        if (teamCurrentHpData[updateCharId] > 0)
        {
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

            playerScript.CharChange(teamIdData[updateCharId]);                  //プレイヤースクリプトにキャラ変更した情報を渡す

            currentChar = updateCharId;
            charId = teamIdData[currentChar];                                   //現在の表のキャラのキャラIDの更新
        }
    }

    //クールタイム関係(裏キャラ専用。表のキャラ除外(PlaeryScriptで管理しているため))
    public void TeamCharRecharge() 
    {
        for(int i = 0; i < 3; i++) 
        {
            if (teamIdData[i] == charId || teamIdData[i] != -1)  //単騎などでチームで欠落しているキャラがいた場合(現状欠員はキャラID-1扱い？) 
            {
                //スキルクールタイム
                if (teamCurrentSkillRechargeData[i] > 0)
                {
                    teamCurrentSkillRechargeData[i] -= Time.deltaTime;
                }
                //スキルクールタイム0未満対処
                if (teamCurrentSkillRechargeData[i] < 0)
                {
                    teamCurrentSkillRechargeData[i] = 0;
                }
                //必殺技クールタイム
                if (teamCurrentSpecialMoveRechargeData[i] > 0)
                {
                    teamCurrentSpecialMoveRechargeData[i] -= Time.deltaTime;
                }
                //必殺技クールタイム0未満対処
                if (teamCurrentSpecialMoveRechargeData[i] < 0)
                {
                    teamCurrentSpecialMoveRechargeData[i] = 0;
                }
            }
        }
    }

    //チーム編成変更
    public void ChangeTeam(int[] updateTeam) 
    {
        teamIdData[0] = updateTeam[0];
        teamIdData[1] = updateTeam[1];
        teamIdData[2] = updateTeam[2];

        GetPlayerData(currentChar);
        SetPlayerData(0);

        playerScript.CharChange(teamIdData[0]);                                 //プレイヤースクリプトにキャラ変更した情報を渡す
    }

    //プレイヤーからデータを取得
    public void GetPlayerData(int currentCharId) 
    {
        teamMaxHpData[currentCharId] = playerScript.MaxHp;                                                //最大HP
        teamCurrentHpData[currentCharId] = playerScript.CurrentHp;                                        //現在HP
        teamCurrentSkillRechargeData[currentCharId] = playerScript.CurrentSkillRecharge;                  //スキルクールタイム
        teamCurrentSpecialMoveRechargeData[currentCharId] = playerScript.CurrentSpecialMoveRecharge;      //スキルクールタイム
    }

    //プレイヤーに対してデータを書き換え
    public void SetPlayerData(int updatedCharId)
    {
        playerScript.MaxHp = teamMaxHpData[updatedCharId];                                                  //最大HP
        playerScript.CurrentHp = teamCurrentHpData[updatedCharId];                                          //現在HP
        playerScript.CurrentSkillRecharge = teamCurrentSkillRechargeData[updatedCharId];                    //スキルクールタイム
        playerScript.CurrentSpecialMoveRecharge = teamCurrentSpecialMoveRechargeData[updatedCharId];        //スキルクールタイム
    }

    //maxHp参照用(getset)
    public int[] TeamMaxHp // プロパティ
    {
        get { return teamMaxHpData; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { teamMaxHpData = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //currentHp参照用(getset)
    public int[] TeamCurrentHp // プロパティ
    {
        get { return teamCurrentHpData; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { teamCurrentHpData = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //teamIdData参照用(getset)
    public int[] TeamIdData // プロパティ
    {
        get { return teamIdData; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { teamIdData = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //CharId参照用(getset)現在のキャラのキャラID
    public int CharId // プロパティ
    {
        get { return charId; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { charId = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //CurrentChar参照用(getset)現在のキャラがチームで何番目か
    public int CurrentChar // プロパティ
    {
        get { return currentChar; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentChar = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
}

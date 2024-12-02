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

        teamCurrentHpData[0] = playerScript.CurrentHp;
        teamCurrentHpData[1] = playerScript.CurrentHp;
        teamCurrentHpData[2] = playerScript.CurrentHp;

        //一人目だった場合
        currentChar = 0;
        sub1Obj.transform.localPosition = new Vector3(25f, 0, 0);
        sub2Obj.transform.localPosition = new Vector3(0, 0, 0);
        sub3Obj.transform.localPosition = new Vector3(0, 0, 0);

        GetPlayerData(currentChar);
        SetPlayerData(0);

        playerScript.CharChange(teamIdData[0]);                                 //プレイヤースクリプトにキャラ変更した情報を渡す
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    //キャラ変更
    public void CharChange(int updateCharId)
    {
        //3キャラに適合した場合
        if (teamCurrentHpData[updateCharId] > 0)
        {
            Debug.Log(updateCharId + "人目に変更");
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
        }
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
}

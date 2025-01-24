using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageNotation : MonoBehaviour
{
    //[SerializeField] GameObject damageNotation;

    //[SerializeField] Text damageText;
    [SerializeField] TextMeshProUGUI damageNotationText;         //ダメージ表記テキスト

    // Start is called before the first frame update
    void Start()
    {
        //重なることを防ぐため
        this.transform.position += new Vector3(Random.Range(-1.0f, 1.0f) * 0.1f, Random.Range(-1.0f, 1.0f) * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ダメージ表記
    public void DamageNotion(int damage, int attribute, bool isAttentionDamage, Vector2 pos)
    {
        /*
        damageText= this.GetComponent<Text>();
        if (attribute >= 0) 
        {
            damageText.text = "" + damage;
        }
        else if (attribute == -1)
        {
            damageText.text = "+" + damage;
        }

        //回復(黄緑)
        if (attribute == -1)
        {
            damageText.color = new Color32(180, 240, 60, 255);
        }
        //物理ダメージ(白)
        else if (attribute == 0) 
        {
            damageText.color = new Color32(240, 240, 240, 255);
        }
        //火属性ダメージ(赤)
        else if (attribute == 1)
        {
            damageText.color = new Color32(240, 10, 10, 255);
        }
        //風属性ダメージ(緑)
        else if (attribute == 2)
        {
            damageText.color = new Color32(120, 240, 160, 255);
        }
        //水属性ダメージ(青)
        else if (attribute == 3)
        {
            damageText.color = new Color32(10, 160, 240, 255);
        }
        //土属性ダメージ(黄)
        else if (attribute == 4)
        {
            damageText.color = new Color32(240, 200, 10, 255);
        }
        //エーテル属性ダメージ(桃)
        else if (attribute == 5)
        {
            damageText.color = new Color32(240, 120, 200, 255);
        }
        //虚空属性ダメージ(藍)
        else if (attribute == 6)
        {
            damageText.color = new Color32(30, 10, 160, 255);
        }

        transform.position = (Vector3)new Vector2(pos.x, pos.y + 0.0f);
        //ダメージ表記のモーション
        StartCoroutine(DamageNotionMotion(isAttentionDamage));
        */
        
        //自身の色の取得
        damageNotationText = this.GetComponent<TextMeshProUGUI>();
        if (attribute >= 0) 
        {
            damageNotationText.text = "" + damage;
        }
        else if (attribute == -1)
        {
            damageNotationText.text = "+" + damage;
        }

        //回復(黄緑)
        if (attribute == -1)
        {
            damageNotationText.color = new Color32(180, 240, 60, 255);
        }
        //物理ダメージ(白)
        else if (attribute == 0) 
        {
            damageNotationText.color = new Color32(240, 240, 240, 255);
        }
        //火属性ダメージ(赤)
        else if (attribute == 1)
        {
            damageNotationText.color = new Color32(240, 10, 10, 255);
        }
        //風属性ダメージ(緑)
        else if (attribute == 2)
        {
            damageNotationText.color = new Color32(120, 240, 160, 255);
        }
        //水属性ダメージ(青)
        else if (attribute == 3)
        {
            damageNotationText.color = new Color32(10, 160, 240, 255);
        }
        //土属性ダメージ(黄)
        else if (attribute == 4)
        {
            damageNotationText.color = new Color32(240, 200, 10, 255);
        }
        //エーテル属性ダメージ(桃)
        else if (attribute == 5)
        {
            damageNotationText.color = new Color32(240, 120, 200, 255);
        }
        //虚空属性ダメージ(藍)
        else if (attribute == 6)
        {
            damageNotationText.outlineColor = new Color32(240, 240, 240, 255);
            damageNotationText.color = new Color32(30, 10, 160, 255);
        }

        transform.position = (Vector3)new Vector2(pos.x, pos.y + 0.0f);
        //ダメージ表記のモーション
        StartCoroutine(DamageNotionMotion(isAttentionDamage));
        
    }

    //ダメージ表記のモーション
    IEnumerator DamageNotionMotion(bool isAttentionDamage)
    {
        float sizeX = this.transform.localScale.x;
        float sizeY = this.transform.localScale.y;

        //会心ダメージ時
        if (isAttentionDamage == true) 
        {
            this.transform.localScale += new Vector3(sizeX / 10 * 30, sizeY / 10 * 30);
        }
        //会心ダメージ時
        for (int i = 0; i < 10; i++)
        {
            if (isAttentionDamage == true) 
            {
                this.transform.localScale -= new Vector3(sizeX / 100 * 28, sizeY / 100 * 28);
            }
            yield return null;
        }

        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        for (int i = 0; i < 20; i++)
        {
            transform.position += (Vector3)new Vector2(0, 0.01f);
            //damageText.color -= new Color(0, 0, 0, 1 / 30f);
            damageNotationText.color -= new Color(0, 0, 0, 1 / 30f);

            yield return null;
        }

        //消滅
        Destroy(this.gameObject);

        yield break;
    }
}

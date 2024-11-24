using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageNotation : MonoBehaviour
{
    //[SerializeField] GameObject damageNotation;

    [SerializeField] Text thisText;

    // Start is called before the first frame update
    void Start()
    {
        //���g�̐F�̎擾
        thisText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�_���[�W�\�L
    public void DamageNotion(int damage, int attribute, bool isAttentionDamage, Vector2 pos)
    {
        Text damageText= this.GetComponent<Text>();
        if (attribute >= 0) 
        {
            damageText.text = "" + damage;
        }
        else if (attribute == -1)
        {
            damageText.text = "+" + damage;
        }

        //��(����)
        if (attribute == -1)
        {
            damageText.color = new Color32(180, 240, 60, 255);
        }
        //�����_���[�W(��)
        else if (attribute == 0) 
        {
            damageText.color = new Color32(240, 240, 240, 255);
        }
        //�Α����_���[�W(��)
        else if (attribute == 1)
        {
            damageText.color = new Color32(240, 10, 10, 255);
        }
        //�������_���[�W(��)
        else if (attribute == 2)
        {
            damageText.color = new Color32(120, 240, 160, 255);
        }
        //�������_���[�W(��)
        else if (attribute == 3)
        {
            damageText.color = new Color32(10, 160, 240, 255);
        }
        //�y�����_���[�W(��)
        else if (attribute == 4)
        {
            damageText.color = new Color32(240, 200, 10, 255);
        }
        //�G�[�e�������_���[�W(��)
        else if (attribute == 5)
        {
            damageText.color = new Color32(240, 120, 200, 255);
        }
        //���󑮐��_���[�W(��)
        else if (attribute == 6)
        {
            damageText.color = new Color32(30, 10, 160, 255);
        }

        transform.position = (Vector3)new Vector2(pos.x, pos.y + 0.0f);
        //�_���[�W�\�L�̃��[�V����
        StartCoroutine(DamageNotionMotion(isAttentionDamage));
    }

    //�_���[�W�\�L�̃��[�V����
    IEnumerator DamageNotionMotion(bool isAttentionDamage)
    {
        float sizeX = this.transform.localScale.x;
        float sizeY = this.transform.localScale.y;

        //��S�_���[�W��
        if (isAttentionDamage == true) 
        {
            this.transform.localScale += new Vector3(sizeX / 8 * 10, sizeY / 8 * 10);
        }
        //��S�_���[�W��
        for (int i = 0; i < 10; i++)
        {
            if (isAttentionDamage == true) 
            {
                this.transform.localScale -= new Vector3(sizeX / 10, sizeY / 10);
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
            thisText.color -= new Color(0, 0, 0, 1 / 30f);

            yield return null;
        }

        //����
        Destroy(this.gameObject);

        yield break;
    }
}
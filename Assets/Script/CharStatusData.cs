using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStatusData : MonoBehaviour
{
    [SerializeField] int[] charCurrentHpData;                   //�S�ẴL�����̌���HP�f�[�^
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;     //�`�[���R���g���[���̃X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        //charCurrentHpData= new int[];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //JSON����f�[�^���擾
    public void GetCharStatusData(int currentCharId)
    {
        
    }
}

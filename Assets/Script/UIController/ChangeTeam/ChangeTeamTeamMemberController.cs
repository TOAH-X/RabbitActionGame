using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamTeamMemberController : MonoBehaviour
{
    [SerializeField] TeamCoutnroller teamCoutnrollerScript;         //teamCoutnroller�̃X�N���v�g

    [SerializeField] GameObject TeamChar1;                          //�`�[���L����1
    [SerializeField] GameObject TeamChar2;                          //�`�[���L����2
    [SerializeField] GameObject TeamChar3;                          //�`�[���L����3

    private int[] teamIdData = new int[3];                          //�`�[���f�[�^(ID)

    // Start is called before the first frame update
    void Start()
    {
        teamIdData = teamCoutnrollerScript.TeamIdData;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

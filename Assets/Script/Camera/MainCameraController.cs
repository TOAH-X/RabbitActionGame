using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;                 //プレイヤーのtransform情報
    [SerializeField] Player playerScript;                       //プレイヤースクリプト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float posX = 0;                                     //x軸(プレイヤーの向いている向きで変わる)
        if (playerScript.IsLookRight) 
        {
            posX = 0.5f;
        }
        else 
        {
            posX = -0.5f;
        }
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + new Vector3(posX, 1.0f, -10), 5.0f * Time.deltaTime);
    }
}

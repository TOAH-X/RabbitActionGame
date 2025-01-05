using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スタートシーンに遷移
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    //ゲームシーンに遷移
    public void LoadGameScene() 
    {
        SceneManager.LoadScene("GameScene");
    }
}

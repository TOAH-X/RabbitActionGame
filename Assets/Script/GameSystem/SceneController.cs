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

    //�X�^�[�g�V�[���ɑJ��
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    //�Q�[���V�[���ɑJ��
    public void LoadGameScene() 
    {
        SceneManager.LoadScene("GameScene");
    }
}

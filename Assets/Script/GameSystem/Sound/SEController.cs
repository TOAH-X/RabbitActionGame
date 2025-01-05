using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SEController;

public class SEController : MonoBehaviour
{
    public static SEController Instance { get; private set; }           //�V���O���g���p
    [SerializeField] AudioSource audioSource;                           //audiosource

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class SESound
    {
        public string name;                 //SE��
        public AudioClip audioClip;         //AudioClip
    }

    public SESound[] seSounds;              //���ʉ����X�g

    //�V���O���g��
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();      //�V���A���C�Y�ɂ��邱��
    }

    public void PlaySound(string soundName)
    {
        SESound sound = System.Array.Find(seSounds, s => s.name == soundName);
        if (sound != null && sound.audioClip != null)
        {
            audioSource.PlayOneShot(sound.audioClip);
        }
        else
        {
            Debug.LogWarning("���ʉ���������Ȃ���");
        }
    }
}

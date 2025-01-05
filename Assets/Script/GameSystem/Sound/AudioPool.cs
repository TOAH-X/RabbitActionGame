using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance { get; private set; }          //シングルトン用
    public int poolSize = 30;                                       //同時に再生できる限界値

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //以下何が書かれているか確認すること

    private Queue<AudioSource> audioSources = new Queue<AudioSource>();

    private void Awake()
    {
        Instance = this;

        //プールの初期化
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Enqueue(source);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSources.Count > 0)
        {
            AudioSource source = audioSources.Dequeue();
            source.clip = clip;
            source.Play();
            StartCoroutine(ReturnToPool(source, clip.length));
        }
    }

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.clip = null;
        audioSources.Enqueue(source);
    }
}

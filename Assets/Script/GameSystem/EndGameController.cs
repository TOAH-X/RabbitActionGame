using UnityEngine;

// Esc�L�[����͂���ƃQ�[�����I�����܂�
// �ǂ��Ȃ����_: ���̃X�N���v�g�����邾���œ����܂�
public class ApplicationQuitter : MonoBehaviour
{
    readonly KeyCode quitKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(quitKey))
        {
            QuitGame();
        }
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }

    // �Q�[���̊J�n���ɃC���X�^���X���쐬���܂�
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CreateInstanceOnLoad()
    {
        var instance = new GameObject(nameof(ApplicationQuitter), typeof(ApplicationQuitter));
        DontDestroyOnLoad(instance);
    }
}
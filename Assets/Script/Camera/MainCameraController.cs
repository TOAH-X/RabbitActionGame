using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;                 //ÉvÉåÉCÉÑÅ[ÇÃtransformèÓïÒ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + new Vector3(0, 1.0f, -10), 5.0f * Time.deltaTime);
    }
}

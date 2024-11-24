using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyLevelNotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //“G‚ÌƒŒƒxƒ‹‚Ì•\Ž¦
    public void LevelNotation(int enemyLevel)
    {
        TextMeshProUGUI enemyLevelNotationText = this.gameObject.GetComponent<TextMeshProUGUI>();
        enemyLevelNotationText.text = "Lv." + enemyLevel.ToString();
    }
}

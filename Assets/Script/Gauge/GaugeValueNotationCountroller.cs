using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeValueNotationCountroller : MonoBehaviour
{
    [SerializeField] Text damageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (damageText == null) 
        {
            damageText = this.GetComponent<Text>();
        }
        
    }

    //HP•\‹L(GageCountroller‚©‚ç)
    public void GaugeValueNotation(int maxValue, int currentValue)
    {
        if (damageText != null) 
        {
            damageText.text = currentValue + "  /  " + maxValue;
        }
    }
}

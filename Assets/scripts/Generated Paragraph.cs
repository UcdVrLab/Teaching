using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratedParagraph : MonoBehaviour
{
    public TMP_Text NumberParagraphText;
    public Slider slider;
    public AIQuery AIQueryScritp;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ValueChanged()
    {
        AIQueryScritp.NumberPoints = slider.value;
        NumberParagraphText.text ="Generated paragraph : "+slider.value;
    }
}

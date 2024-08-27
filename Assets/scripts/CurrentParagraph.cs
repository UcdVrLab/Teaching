using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentParagraph : MonoBehaviour
{
    public TMP_Text NumberParagraphText;
    public AIQuery aiQuery;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NumberParagraphText.text = "Current Paragraph : " + aiQuery.NumberActualPart;


    }
}

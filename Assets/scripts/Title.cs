using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    public string SummaryComplete;
    public StartLesson startLesson;
    public AIQuery AIQueryScritp;
    public TMP_Text TitleText;
    public TMP_Text SummaryText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        SummaryText.text=SummaryComplete;
        if (AIQueryScritp.NumberActualPart == 0)
        {
            TitleText.text = "";
        }

    }
    public void LineSeletion()
    {
        
        TitleText.text = startLesson.SummaryLinesListWithoutVoid[AIQueryScritp.NumberActualPart-1];

    }
}

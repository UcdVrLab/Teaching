using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    public string SummaryComplete;
    public string[] SummaryLines;
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
        SummaryLines = SummaryComplete.Split('\n');
        TitleText.text = SummaryLines[AIQueryScritp.NumberActualPart+1];

    }
}

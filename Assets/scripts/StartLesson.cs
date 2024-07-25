using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLesson : MonoBehaviour
{
    public AIQuery aiQuery;

    public string SummaryComplete;
    //public string[] SummaryLines;
    private List<string> SummaryLinesListWithVoid = new List<string>();
    public List<string> SummaryLinesListWithoutVoid = new List<string>();
    public string userQuery;
    public float maxParagraph;
    public bool IAPrepared;
    public float compteur = 0f;
    public float compteurMax = 5f;
    public int iter;
    // Start is called before the first frame update
    void Start()
    {
        IAPrepared = false;
        iter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PreparingAiquery();
        LaunchAI();


    }
    public void ResetAiPrepared()
    {
        IAPrepared = false;
        SummaryLinesListWithoutVoid.Clear();
        SummaryLinesListWithVoid.Clear();
        iter = 0;
        aiQuery.ResponseWhitoutTriggerReceived = false;
    }
    public void LaunchAI()
    {
        compteur += Time.deltaTime;
        if (compteur > compteurMax)
        {
            compteur = 0;
            if (IAPrepared == true)
            {
                if (iter < maxParagraph)//bouton pressed and sending ia true et iter<maxparagraph
                {
                    
                    //audios
                    //images
                    //summary
                    userQuery = SummaryLinesListWithoutVoid[iter];
                    aiQuery.StartAIQueryDevelop(userQuery);
                    aiQuery.FictivNumberActualPart += 1;
                    iter++;
                }
                else //iter==maxparagraph
                {
                    IAPrepared = false;
                    iter = 0;
                }
            }
        }
    }
    public void PreparingAiquery()//response whithout trigger received need to modify
    {
        if (aiQuery.ResponseWhitoutTriggerReceived)
        {
            aiQuery.ResponseWhitoutTriggerReceived = false;
            maxParagraph = aiQuery.NumberPoints;
            SummaryComplete = aiQuery.ResponseWhitoutTrigger;
            for (int i = 0; i < SummaryComplete.Split('\n').Length; i++)
            {  
                SummaryLinesListWithVoid.Add(SummaryComplete.Split('\n')[i]);
                //Debug.Log("AHHHHHHHHHHHHH" + SummaryComplete);
                if (SummaryLinesListWithVoid[i] == "")
                {
                }
                else
                {
                    SummaryLinesListWithoutVoid.Add(SummaryLinesListWithVoid[i]);
                }
            }
            IAPrepared = true;
        }
    }
}

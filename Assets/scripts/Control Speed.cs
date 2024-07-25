using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples.Whisper
{

public class ControlSpeed : MonoBehaviour
{
    public GameObject NextPartButton;
    public VoiceRecorder voiceRecorder;
    public AIQuery aiquery;
    public CurrentParagraph currentParagraph;
    public bool FirstParagraphAlreadyPlay;
        public bool RecordOn;
    // Start is called before the first frame update
    void Start()
    {
            FirstParagraphAlreadyPlay = false;
            RecordOn = true;
    }

    // Update is called once per frame
    void Update()
    {
            LlamaControlSpeed();
            FirstParagraphButton();


    }
    public void FirstParagraphcontrol()
    {
        FirstParagraphAlreadyPlay = false;
    }
        public void LlamaControlSpeed()
    {
        if (NextPartButton)
        {
            if (voiceRecorder.assistantMessageListe[aiquery.NumberActualPart+1]=="Waiting...")
            {
                NextPartButton.SetActive(false);

            }
                else
                {
                    NextPartButton.SetActive(true);
                }

            }
    }
    public void FirstParagraphButton()
    {
            if (!FirstParagraphAlreadyPlay)
            {
                if (voiceRecorder.assistantMessageListe[0]!="Waiting...")
                {
                    FirstParagraphAlreadyPlay = true;
                    currentParagraph.PlayAudioActualPart();
                }
                if (voiceRecorder.assistantMessageListe[0] == "Waiting...")
                {
                    FirstParagraphAlreadyPlay = false;

                }
            }

    }
}
}

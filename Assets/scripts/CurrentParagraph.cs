using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Samples.Whisper
{



    public class CurrentParagraph : MonoBehaviour
    {
        public TMP_Text NumberParagraphText;
        public TMP_Text SummaryText;

        public AIQuery aiQuery;
        public DALLERequestManager dallERequestManager;
        public TextToSpeech textToSpeech;
        public VoiceRecorder voiceRecorder;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            NumberParagraphText.text = "Current Paragraph : " + aiQuery.NumberActualPart;
            SummaryText.text = voiceRecorder.assistantMessageListe[aiQuery.NumberActualPart];


        }
        public void NextPartPressed()
        {
            //image
            if (aiQuery.NumberActualPart >= 0 && aiQuery.NumberActualPart < dallERequestManager.GameObjectImage.Length)
            {

                foreach (GameObject gameObject in dallERequestManager.GameObjectImage)
                {
                    gameObject.SetActive(false);
                }
                dallERequestManager.GameObjectImage[aiQuery.NumberActualPart].SetActive(true);
            }

            //audio
            textToSpeech.audioSource.clip = textToSpeech.audioClips[aiQuery.NumberActualPart];
            textToSpeech.audioSource.Play();

            //summary
            //reset index summary

        }
        public void PlayAudioActualPart()
        {
            textToSpeech.audioSource.clip = textToSpeech.audioClips[aiQuery.NumberActualPart];
            textToSpeech.audioSource.Play();
        }
    }
}
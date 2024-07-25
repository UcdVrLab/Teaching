using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System;
namespace Samples.Whisper
{
    public class VoiceRecorder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private TextMeshProUGUI summary;

        public AIQuery aiQuery;

        


        private string apiUrl = "http://localhost:1234/v1/chat/completions";

        
        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        
        private AudioClip clip;
        public bool isRecording = false;
        private float time;
        public string assistantMessage;
        public string[] assistantMessageListe;
        public int IndexAssistantMessage=0;

        private void Start()
        {
            if (aiQuery != null) {
                aiQuery.OnAIResponseReceived += HandleAIResponse;
                } 
                else {
                Debug.LogError("AIQuery reference not set on VoiceRecorder.");
                }
                //StartRecording(); HERE
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        public void StartRecording()
        {
            if (isRecording) return;

            isRecording = true;
            Debug.Log("it's recording");

            clip = Microphone.Start(null, false, duration, 44100); 
        }

        public IEnumerator EndRecording()
        {
            
            Microphone.End(null);
            yield return null;
            
            byte[] data = SaveWav.Save(fileName, clip);

            WWWForm form = new WWWForm();
            form.AddBinaryData("file", data, "audio.wav", "audio/wav");

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/transcribe", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    message.text = "Error: " + www.error;
                }
                else
                {
                    Debug.Log("Transcription successful!");
                    aiQuery.StartAIQueryBulletPoint(www.downloadHandler.text);//BulletPoint
                    
                    
                }
            }  
    }
        
        private void HandleAIResponse(string animationTrigger, string responseWithoutTrigger) {
            StartCoroutine(GenerateSummary(responseWithoutTrigger)); 
        }


        private IEnumerator GenerateSummary(string llmResponse) {
            Msg systemMessage = new Msg { role = "system", content = @"
            Your job is to summarise text.
            DO NOT try to summarise a single sentence or conversational text. 
            Summarise this text quickly into very short bullet points (a couple of words will suffice). 
            Your summary must always be shorter than what you're summarising." };
            Msg userMessage = new Msg { role = "user", content = llmResponse };

            RequestBody requestBody = new RequestBody {
                model = "local-model",
                messages = new Msg[] { systemMessage, userMessage },
                temperature = 0.7
            };

        string json = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else {


            string responseJson = www.downloadHandler.text;

                
            
            AIResponse response = JsonUtility.FromJson<AIResponse>(responseJson);

            if (response.choices != null && response.choices.Length > 0 && response.choices[0].message != null)
            {
                assistantMessageListe[IndexAssistantMessage] = response.choices[0].message.content;
                IndexAssistantMessage++;
                
                //Debug.Log("Assistant's Response: " + assistantMessageListe[IndexAssistantMessage]);
            }
            else
            {
                Debug.LogError("Invalid response format.");
            }
        
            }
        }
        }

        private void OnDestroy() {
        if (aiQuery != null) {
            aiQuery.OnAIResponseReceived -= HandleAIResponse;
            }
        }


        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    StartCoroutine(EndRecording());
                }
            }
        }
        public void ResetAsistantMessage()
        {
            for (int i = 0; i < assistantMessageListe.Length; i++)
            {
                assistantMessageListe[i] = "Waiting...";

            }
            IndexAssistantMessage = 0;

        }
    }
}

[Serializable]
public class AIResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public int index;
    public Msg message;
    public string finish_reason;
}

[Serializable]
public class Msg
{
    public string role;
    public string content;
}

[Serializable]
public class RequestBody
{
    public string model;
    public Msg[] messages;
    public double temperature;
}


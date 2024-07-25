using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
//using UnityEditor.Scripting.Python;
public class TextToSpeech : MonoBehaviour
{
    private string apiKey;
    private string apiUrl;
    public AIQuery aiQuery;
    byte[] audioData;


    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioClip currentAudio;


    void Start()
    {
        LoadApiKey();
        apiUrl = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;
        if (aiQuery != null)
        {
            aiQuery.OnAIResponseReceived += ConvertResponseToSpeech;//on converti la reponse de gpt en speech
        } else {
            Debug.Log("AI QUery Null");
        }    
    }

    private void ConvertResponseToSpeech(string animationTrigger, string responseWithoutTrigger)
    {
        StartCoroutine(ConvertTextToSpeech(responseWithoutTrigger));
    }

     private void LoadApiKey()
     {
         string configFilePath = ".//Config.json"; // Will need to change this as well for new API keys
        

         try
         {
             string json = File.ReadAllText(configFilePath);
             Config config = JsonUtility.FromJson<Config>(json);
             apiKey = config.googleApiKey;
         }
         catch (Exception e)
         {
             Debug.LogError("Failed to read API key: " + e.Message);
         }
     }
    
            

    

    public IEnumerator ConvertTextToSpeech(string text)
    {
        var requestData = new TextToSpeechRequest
        {
            input = new Input { text = text },
            voice = new Voice { languageCode = "en-US", ssmlGender = "NEUTRAL" },
            audioConfig = new AudioConfig { audioEncoding = "LINEAR16" } 
        };

        string json = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string responseString = www.downloadHandler.text; //is it the text from AI ?
                var response = JsonUtility.FromJson<TextToSpeechResponse>(responseString);

                audioData = Convert.FromBase64String(response.audioContent);

                GenerateAudio(audioData,aiQuery.FictivNumberActualPart);
                
            }
        }
        
    }
  
    private void GenerateAudio(byte[] audioData,int numberPart)
    {
        string filePath = Path.Combine(Application.temporaryCachePath, "ttsOutput" + numberPart + ".wav");
        File.WriteAllBytes(filePath, audioData);
        StartCoroutine(LoadAndPlayAudio(filePath));
    }

    private IEnumerator LoadAndPlayAudio(string filePath)
    {
        //Debug.Log("file://" + filePath);//here



        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                audioClips.Add(clip);
                //audioSource.clip = audioClips[aiQuery.NumberActualPart];
                //audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to load audio clip: " + uwr.error);
            }
        }
    }
    public void ResetAudio()
    {
        audioClips.Clear();
    }
  
    


  



}



[Serializable]
public class TextToSpeechRequest
{
    public Input input;
    public Voice voice;
    public AudioConfig audioConfig;
}

[Serializable]
public class Input
{
    public string text;
}

[Serializable]
public class Voice
{
    public string languageCode;
    public string ssmlGender;
}

[Serializable]
public class AudioConfig
{
    public string audioEncoding;
}

[Serializable]
public class TextToSpeechResponse
{
    public string audioContent; 
}

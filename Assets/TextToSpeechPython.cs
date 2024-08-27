// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using System.IO;

// public class TextToSpeechPython : MonoBehaviour
// {
//     private string apiUrl = "http://localhost:5000/convert"; // URL of the local Python server
//     public AIQuery aiQuery;
//     public AudioSource audioSource;

//     void Start()
//     {
//         if (aiQuery != null)
//         {
//             aiQuery.OnAIResponseReceived += ConvertResponseToSpeech;
//         }
//         else
//         {
//             Debug.Log("AI Query Null");
//         }
//     }

//     private void ConvertResponseToSpeech(string animationTrigger, string responseWithoutTrigger)
//     {
//         StartCoroutine(ConvertTextToSpeechCoroutine(responseWithoutTrigger));
//     }

//     private IEnumerator ConvertTextToSpeechCoroutine(string text)
//     {
//         var requestData = new { text = text };
//         string json = JsonUtility.ToJson(requestData);

//         using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, json))
//         {
//             www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
//             www.downloadHandler = new DownloadHandlerBuffer();
//             www.SetRequestHeader("Content-Type", "application/json");

//             yield return www.SendWebRequest();

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("Error: " + www.error);
//             }
//             else
//             {
//                 byte[] audioData = www.downloadHandler.data;
//                 PlayAudio(audioData);
//             }
//         }
//     }

//     private void PlayAudio(byte[] audioData)
//     {
//         string filePath = Path.Combine(Application.temporaryCachePath, "ttsOutput.wav");
//         File.WriteAllBytes(filePath, audioData);
//         StartCoroutine(LoadAndPlayAudio(filePath));
//     }

//     private IEnumerator LoadAndPlayAudio(string filePath)
//     {
//         using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
//         {
//             yield return uwr.SendWebRequest();

//             if (uwr.result == UnityWebRequest.Result.Success)
//             {
//                 AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
//                 audioSource.clip = clip;
//                 audioSource.Play();
//             }
//             else
//             {
//                 Debug.LogError("Failed to load audio clip: " + uwr.error);
//             }
//         }
//     }
// }

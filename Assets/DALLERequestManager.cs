using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DALLERequestManager : MonoBehaviour
{
    public GameObject[] GameObjectImage;
    //public Sprite canvasImage;
    public int ImageIndex=0;

    string DALLE_API_KEY;

    private void LoadApiKey()
    {
        string configFilePath = ".//Config.json"; // Change this and anywhere else it exists if using ur own api key

        try
        {
            string json = File.ReadAllText(configFilePath);
            Config config = JsonUtility.FromJson<Config>(json);
            DALLE_API_KEY = config.openaiApiKey;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to read API key: " + e.Message);
        }
    }

    public void GenerateImage(string prompt)
    {
    LoadApiKey();
    StartCoroutine(CallDalleAPI("Review the following text and generate a simple and relevant explanatory diagram. There should be no text in the image.: " + prompt));
    }

        private void Start()
    {
        AIQuery aiQuery = FindObjectOfType<AIQuery>();
        if (aiQuery != null)
        {
            aiQuery.OnAIResponseReceived += HandleAIResponseReceived;
        }
        else
        {
            Debug.LogError("AIQuery component not found in the scene.");
        }
    }

    private void HandleAIResponseReceived(string animationTrigger, string responseWithoutTrigger)
    {
        GenerateImage(responseWithoutTrigger);
    }

     IEnumerator CallDalleAPI(string prompt)
    {
        string url = "https://api.openai.com/v1/images/generations";
        DalleRequest requestJson = new DalleRequest
        {
            prompt = prompt,
            n = 1,
            model = "dall-e-3"
        };

        string jsonBody = JsonUtility.ToJson(requestJson);
        //Debug.Log(jsonBody);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + DALLE_API_KEY);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("DALL·E 3 response received!");
                Debug.Log("Raw Response: " + www.downloadHandler.text);
                DalleResponse response = JsonUtility.FromJson<DalleResponse>(www.downloadHandler.text);

                if (response.data != null && response.data.Length > 0)
                {
                    string imageUrl = response.data[0].url;
                    //Debug.Log("Image URL: " + imageUrl);
                    StartCoroutine(DownloadAndDisplayImage(imageUrl));
                }
                else
                {
                    Debug.LogError("Data array is null or empty.");
                }
            }
        }
    }


    IEnumerator DownloadAndDisplayImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            Debug.LogError("Image URL is null or empty.");
            yield break;
        }
        //Debug.Log("Downloading image from URL: " + imageUrl);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to download image: " + www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            if (texture == null)
            {
                Debug.LogError("Texture is null.");
                yield break;
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            if (sprite == null)
            {
                Debug.LogError("Sprite is null.");
                yield break;
            }
            Image ImageTemp=GameObjectImage[ImageIndex].GetComponent<Image>();
            if (ImageTemp != null)
            {
                //canvasImage[ImageIndex]= sprite;
                ImageTemp.sprite = sprite;
                ImageIndex++;
                //Debug.Log(ImageIndex);
            }
            else
            {
                Debug.LogError("Image component not assigned in the Inspector.");
            }
        }

    }
    public void ResetImages()
    {
        ImageIndex = 0;
    }

}

[System.Serializable]
public class DalleRequest
{
    public string prompt;
    public int n;
    public string model;
}

[System.Serializable]
public class DalleResponse
{
    public long created;
    public DalleImageData[] data;
}

[System.Serializable]
public class DalleImageData
{
    public string url;
}


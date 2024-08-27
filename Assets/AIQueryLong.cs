using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using com.studios.taprobana;
using System.IO;

// This article showed me how to use the API: https://medium.com/@achinthabandara1/openai-unity-509fb6173b87
public class AIQueryLong : MonoBehaviour
{
    private string apiKey;
    private ChatCompletionsApi chatCompletionsApi;

    public delegate void AIResponseReceived(string animationTrigger, string responsWithoutTrigger);
    public event AIResponseReceived OnAIResponseReceived;


    public void StartAIQuery(string userQuery) {
    LoadApiKey();


    chatCompletionsApi = new(apiKey);
    chatCompletionsApi.ConversationHistoryMemory = 5;
    chatCompletionsApi.SetSystemMessage(@"
    You are here to help explain concepts and answer questions.
    Your answer must be at least 500 words
    When responding, please select an appropriate animation from the following list: Thinking, Explaining, FingerPointing, HandsClasped, Height, Juxtaposition, OneOrTheOther, RubbingHandsTogether, SizeSmallToLarge. 
    The exact name of the animation should be the first word in your response, followed by a comma.
    You must start every response in this format!");

    SendRequestToAI(userQuery);
    }
    private void LoadApiKey()
    {
        string configFilePath = "C://Users//cave//Documents//Valentin Mikey projet//FYP-master//Config.json"; // Change this and anywhere else it exists if using ur own api key
        

        try
        {
            string json = File.ReadAllText(configFilePath);
            Config config = JsonUtility.FromJson<Config>(json);
            apiKey = config.openaiApiKey;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to read API key: " + e.Message);
        }
    }

public async void SendRequestToAI(string userQuery)
{
    try
    {
        ChatCompletionsRequest chatCompletionsRequest = new ChatCompletionsRequest();
        Message message = new(Roles.USER, userQuery);

        chatCompletionsRequest.AddMessage(message);

        Debug.Log("Sending request to AI");


        ChatCompletionsResponse res = await chatCompletionsApi.CreateChatCompletionsRequest(chatCompletionsRequest);
        string fullResponse = res.GetResponseMessage();
        Debug.Log("Full Responde : "+fullResponse);

        string[] words = fullResponse.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        string animationTrigger = words.Length > 0 ? words[0] : "";
        string responseWithoutTrigger = words.Length > 1 ? fullResponse.Substring(fullResponse.IndexOf(' ') + 1) : "";
        Debug.Log("Response withoutTrigger : "+responseWithoutTrigger);

        OnAIResponseReceived?.Invoke(animationTrigger, responseWithoutTrigger);
    }
    catch (Exception exception) 
    {
        Debug.LogError(exception);
    }
}


}

[Serializable]
public class Config2
{
    public string openaiApiKey;
    public string googleApiKey;
}




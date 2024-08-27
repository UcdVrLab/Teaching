using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using com.studios.taprobana;
using System.IO;

// This article showed me how to use the API: https://medium.com/@achinthabandara1/openai-unity-509fb6173b87
public class AIQuery : MonoBehaviour
{
    private string apiKey;
    private ChatCompletionsApi chatCompletionsApi;
    public float NumberPoints;
    public int NumberActualPart=0;
    public string ResponseWhitoutTrigger;
    public Title titlseScript;

    public bool ResponseWhitoutTriggerReceived = false;

    public delegate void AIResponseReceived(string animationTrigger, string responsWithoutTrigger);
    public event AIResponseReceived OnAIResponseReceived;


    public void StartAIQueryBulletPoint(string userQuery) {
    LoadApiKey();


    chatCompletionsApi = new(apiKey);
    chatCompletionsApi.ConversationHistoryMemory = 5;
    //You are here to help explain concepts and answer questions.
    //Your answer must be  1200 words
    chatCompletionsApi.SetSystemMessage(@"
    
    When responding, please select an appropriate animation from the following list: Thinking, Explaining, FingerPointing, HandsClasped, Height, Juxtaposition, OneOrTheOther, RubbingHandsTogether, SizeSmallToLarge. 
    The exact name of the animation should be the first word in your response, followed by a comma.
    You must start every response in this format!");

    SendRequestToAIBulletPoint("gives a plan in ONLY" + NumberPoints + " points that answers the following question if they were developed : "+userQuery); //Bullet Point 
    }
    public void AddIndexPart()
    {
        NumberActualPart += 1;
        

    }
    public void StartAIDevelopButton()
    {
        StartAIQueryDevelop(ResponseWhitoutTrigger);
    }
    public void ResetIndexPart()
    {
        NumberActualPart = 0;
    }
    public void StartAIQueryDevelop(string userQuery)
    {
        LoadApiKey();
        chatCompletionsApi = new(apiKey);
        chatCompletionsApi.ConversationHistoryMemory = 5;
        //You are here to help explain concepts and answer questions.
        //Your answer must be  1200 words
        chatCompletionsApi.SetSystemMessage(@"
    You are here to help explain concepts and answer questions.
    Your answer must be  1200 words
    When responding, please select an appropriate animation from the following list: Thinking, Explaining, FingerPointing, HandsClasped, Height, Juxtaposition, OneOrTheOther, RubbingHandsTogether, SizeSmallToLarge. 
    The exact name of the animation should be the first word in your response, followed by a comma.
    You must start every response in this format!");
        
            SendRequestToAIDevelop("Developpe ONLY the point number " + NumberActualPart.ToString() +userQuery); 
    }
    private void LoadApiKey()
    {
        string configFilePath = ".//Config.json"; // Change this and anywhere else it exists if using ur own api key
        

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

public async void SendRequestToAIBulletPoint(string userQuery)
{
    try
    {
        ChatCompletionsRequest chatCompletionsRequest = new ChatCompletionsRequest();
        Message message = new(Roles.USER, userQuery);
        string fullResponse;

        chatCompletionsRequest.AddMessage(message);

        Debug.Log("Sending request to AI");


            ChatCompletionsResponse res = await chatCompletionsApi.CreateChatCompletionsRequest(chatCompletionsRequest);
            fullResponse = res.GetResponseMessage();
            Debug.Log("Full Responde : "+fullResponse);
            ;
            

        string[] words = fullResponse.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        string animationTrigger = words.Length > 0 ? words[0] : "";
        ResponseWhitoutTrigger = words.Length > 1 ? fullResponse.Substring(fullResponse.IndexOf(' ') + 1) : "";
        titlseScript.SummaryComplete = ResponseWhitoutTrigger;
        ResponseWhitoutTriggerReceived = true;
        //Debug.Log("Response withoutTrigger : "+responseWithoutTrigger);

        OnAIResponseReceived?.Invoke(animationTrigger, ResponseWhitoutTrigger);//Responsewithout trigger = without the first word
    }
    catch (Exception exception) 
    {
        Debug.LogError(exception);
    }
}
    public async void SendRequestToAIDevelop(string userQuery)
    {
        try
        {
            string fullResponse;
            ChatCompletionsRequest chatCompletionsRequest = new ChatCompletionsRequest();
            Message message = new(Roles.USER, userQuery);

            chatCompletionsRequest.AddMessage(message);

            Debug.Log("Sending request to AI Develop");
            ResponseWhitoutTriggerReceived = false;




            ChatCompletionsResponse res = await chatCompletionsApi.CreateChatCompletionsRequest(chatCompletionsRequest);
            fullResponse = res.GetResponseMessage();
            
            


            string[] words = fullResponse.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string animationTrigger = words.Length > 0 ? words[0] : "";
            string responseWithoutTrigger = words.Length > 1 ? fullResponse.Substring(fullResponse.IndexOf(' ') + 1) : "";
            //Debug.Log("Response withoutTrigger : "+responseWithoutTrigger);

            OnAIResponseReceived?.Invoke(animationTrigger, responseWithoutTrigger);//Responsewithout trigger = without the first word
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }
 



}

[Serializable]
public class Config
{
    public string openaiApiKey;
    public string googleApiKey;
}




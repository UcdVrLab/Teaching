using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class WhisperServerLauncher : MonoBehaviour
{
    private Process whisperServiceProcess;
    private Process localLLMServiceProcess;


    void Start()
    {
        StartWhisperService();
        UnityEngine.Debug.Log("Whisper Launched");
    }

    void OnApplicationQuit()
    {
        if (whisperServiceProcess != null)
        {
            whisperServiceProcess.Kill();
            whisperServiceProcess.Dispose();
        }
    }

    void StartWhisperService()
    {
        whisperServiceProcess = new Process();
        whisperServiceProcess.StartInfo.FileName = @"C:/Users/cave/AppData/Local/Programs/Python/Python312/python.exe";// Will need to change this on different devices 

        string scriptPath = "./Assets/Whisper Script/whisper_server.py"; 
        UnityEngine.Debug.Log("launch python server");      
        scriptPath = scriptPath.Replace("\\", "/");

        whisperServiceProcess.StartInfo.Arguments = "\"" + scriptPath + "\"";
        whisperServiceProcess.StartInfo.CreateNoWindow = false;
        whisperServiceProcess.StartInfo.UseShellExecute = true;
        whisperServiceProcess.Start();
    }
    // void StartlocalLLmService()
    // {
    //     localLLMServiceProcess = new Process();
    //     localLLMServiceProcess.StartInfo.FileName = "python";

    //     string scriptPath = "C:/Users/cave/Documents/Valentin Mikey projet/FYP-master/Assets/Whisper Script/TTS localpy.py"; // Will need to change this on different devices
        
    //     scriptPath = scriptPath.Replace("\\", "/");

    //     localLLMServiceProcess.StartInfo.Arguments = "\"" + scriptPath + "\"";
    //     localLLMServiceProcess.StartInfo.CreateNoWindow = true;
    //     localLLMServiceProcess.StartInfo.UseShellExecute = false;
    //     localLLMServiceProcess.Start();
    // }
}

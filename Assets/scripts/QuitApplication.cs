using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Quit()
    {
#if UNITY_EDITOR
        // Si on est dans l'�diteur Unity, arr�tez simplement l'ex�cution du jeu
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Sinon, quittez l'application
        Application.Quit();
#endif
    }
}

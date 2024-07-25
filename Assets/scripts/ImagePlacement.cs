using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePlacement : MonoBehaviour
{
    public Toggle toggle;
    public GameObject SpawnPointClose;
    public GameObject SpawnPointNotClose;
    public GameObject Image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void ToggleGestion()
    {
        if (toggle.isOn)
        {
            Image.transform.position=SpawnPointClose.transform.position;
        }
        else
        {
            Image.transform.position = SpawnPointNotClose.transform.position;
        }

    }
}

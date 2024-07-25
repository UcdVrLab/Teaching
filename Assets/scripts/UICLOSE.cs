using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICLOSE : MonoBehaviour
{
    public GameObject FrontUI;
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UIClose()
    {
        if (toggle.isOn)
        {

        FrontUI.SetActive(false);
        }
        else
        {
        FrontUI.SetActive(true);

        }
    }
}

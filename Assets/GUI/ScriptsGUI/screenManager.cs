using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class screenManager : MonoBehaviour
{
    public GameObject[] screens;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchScreens(String from, String to)
    {
        bool fromDisabled = false;
        bool toEnabled = false;

        foreach(GameObject a in screens)
        {
            if (fromDisabled && toEnabled) break;

            if(a.name == from)
            {
                a.SetActive(false);
                fromDisabled = true;
            }

            if(a.name == to)
            {
                a.SetActive(true);
                toEnabled = true;
            }
        }
    }

    
}

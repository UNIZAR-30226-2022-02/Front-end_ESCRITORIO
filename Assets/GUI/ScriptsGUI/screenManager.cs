using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class screenManager : MonoBehaviour
{   
    public GameObject[] menuScreens;
    public GameObject fondoMenus;
    public GameObject tableroScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchScreens(String from, String to)
    {
        bool fromDisabled = false;
        bool toEnabled = false;

        
        // Cambio de partida a menus y viceversa
        if (tableroScreen.name == from){
            tableroScreen.SetActive(false);
            fondoMenus.SetActive(true);
            return;
        }

        if (tableroScreen.name == to){
            tableroScreen.SetActive(true);
            fondoMenus.SetActive(false);
            return;
        }

        // Cambio dentro de menus
        foreach(GameObject a in menuScreens)
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

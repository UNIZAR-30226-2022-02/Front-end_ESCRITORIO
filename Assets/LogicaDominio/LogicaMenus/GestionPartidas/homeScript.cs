using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeScript : MonoBehaviour
{
    public Button shopButton, historyButton, newGameButton, searchGameButton, joinGameButton, backButton;
    public Text userName;
    public InputField gameCode;
    public Dropdown optionTipo,optionPlayers;
    public GameObject BordeCrearPartida;

    private screenManager sm;
    // Start is called before the first frame update
    void Start()
    {
        shopButton.onClick.AddListener(shop);
        historyButton.onClick.AddListener(history);
        newGameButton.onClick.AddListener(newGame);
        //backButton.onClick.AddListener(quitarCrearPartida);
        joinGameButton.onClick.AddListener(joinGame);

        sm = transform.parent.parent.GetComponent<screenManager>();
    }

    // Update is called once per frame
    void shop()
    {
        sm.switchScreens(this.name, "Tienda");
    }

    void history()
    {
        sm.switchScreens(this.name, "Historial");
    }

    void newGame()
    {
        BordeCrearPartida.SetActive(true);
        
        //Poner todo lo de meter datos en los dropdown

    }

    void quitarCrearPartida()
    {
        BordeCrearPartida.SetActive(false);
    }

    void joinGame()
    {
        sm.switchScreens(this.name, "Tablero");
    }
}

using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeScript : MonoBehaviour
{
    public Button shopButton, historyButton, newGameButton, searchGameButton, joinGameButton;
    public Text userName;
    public InputField gameCode;
    public Dropdown option;

    private screenManager sm;
    // Start is called before the first frame update
    void Start()
    {
        shopButton.onClick.AddListener(shop);
        historyButton.onClick.AddListener(history);
        //newGameButton.onClick.AddListener(newGame);
        //searchGameButton.onClick.AddListener(SearchGame);
        //joinGameButton.onClick.AddListener(joinGame);

        sm = transform.parent.GetComponent<screenManager>();
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
}

using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class registroScript : MonoBehaviour
{

    public InputField mailInput, passwordInput, repeatPasswordInput,userInput;
    public Button botonRegistro, botonBack; //boton de ir al login
    private screenManager sm;
    // Start is called before the first frame update
    void Start()
    {
        botonRegistro.onClick.AddListener(registro);
        botonBack.onClick.AddListener(irLogin);

        sm = transform.parent.GetComponent<screenManager>();
    }

  private void registro()
  {
    String mail = mailInput.text;
    String pass =  passwordInput.text;
    String RepeatPass =  repeatPasswordInput.text;
    String user =  userInput.text;

    /*
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
    HttpWebResponse response = (HttpWebResponse)request.getResponse();
    StreamReader reader = new StreamReader(response.getResponseStream());
    string json = reader.readToEnd();
    JsonUtility.FromJson<TIPO-DE-DATO-RECIBIDO>(json);
    */
    
    sm.switchScreens(this.name, "Home");
  }

  private void irLogin(){
    sm.switchScreens(this.name, "Login");
  }
}

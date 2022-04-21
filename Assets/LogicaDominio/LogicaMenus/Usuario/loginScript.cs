using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class loginScript : MonoBehaviour
{
  public InputField mailInput, passwordInput;

  public Button botonLogin, botonRegistro;

  private screenManager sm;


  void Start(){
    botonLogin.onClick.AddListener(login);    
    botonRegistro.onClick.AddListener(irRegistro); 

    sm = transform.parent.GetComponent<screenManager>();
  }

  private void login()
  {
    String mail = mailInput.text;
    String pass =  passwordInput.text;

    /*
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
    HttpWebResponse response = (HttpWebResponse)request.getResponse();
    StreamReader reader = new StreamReader(response.getResponseStream());
    string json = reader.readToEnd();
    JsonUtility.FromJson<TIPO-DE-DATO-RECIBIDO>(json);
    */
    
    sm.switchScreens(this.name, "Home");
  }

  private void irRegistro()
  {
    sm.switchScreens(this.name, "Registro");
  }

  
}

using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class loginScript : MonoBehaviour
{
  public string nombreUsuario;
  public InputField usernameInput, passwordInput;
  public Button botonLogin, botonRegistro;
  public GameObject errorMessage;

  private screenManager sm;

  void Start(){
    botonLogin.onClick.AddListener(loginAsync);    
    botonRegistro.onClick.AddListener(irRegistro); 

    sm = transform.parent.parent.GetComponent<screenManager>();
  }

  private void  loginAsync(){
    String user = usernameInput.text;
    String pass = passwordInput.text;

    StartCoroutine((rawLogin(user, pass)));
  }
  private IEnumerator rawLogin(String username, String password)
  {
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);
    UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/login", form);
    
    yield return req.Send();
    
    //Se ha logueado correctamente
    if (req.error == null)
    {
      String res = req.downloadHandler.text;

      if (res == "OK"){
        Debug.Log("Sesión Iniciada");

        // Inicia sesion Socket.IO
        WebSocketHandler wsHandler = transform.parent.parent.GetComponent<WebSocketHandler>();
        wsHandler.registrarme(username);

        //Guardo en el fichero de varaibles globales el nombre de usuario que se ha introducido
        transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setUsername(username);
        sm.switchScreens(this.name, "Home");
      }
      else
      {
        errorMessage.SetActive(true);
      }
      
    }
    else
    {
      Debug.Log("Error: " + req.error);
    }
  }

  private void irRegistro()
  {
    sm.switchScreens(this.name, "Registro");
  }

  
}

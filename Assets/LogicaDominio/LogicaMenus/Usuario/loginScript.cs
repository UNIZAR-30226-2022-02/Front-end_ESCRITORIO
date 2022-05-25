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

  [System.Serializable]
  public class Datos{
    public int mapaSel,fichaSel, enPartida;
    public int[] objetosComprados;
  }

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

        //LLamar al servidor para actualizar los datos del usuario
        WWWForm form2 = new WWWForm();
        form2.AddField("username", username);

        UnityWebRequest respuesta = UnityWebRequest.Post("serverrisk.herokuapp.com/login/datos", form2);
        yield return respuesta.Send();

        //mapaComp, mapaSel,fichComp,fichaSele,enPartida;
        string resultado = respuesta.downloadHandler.text;
        Datos data = JsonUtility.FromJson<Datos>(resultado);

        //Actualizo la variable estoyEnPartida de las variables de entorno
        if(data.enPartida != -1){
          transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setEstoyEnPartida(true);
        }

        //Actualizo los objetos comprados en las variables de entorno
        for(int i = 0; i<2; i++){
          if(data.objetosComprados[i] == 1){
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setMapaComprado();
          }
          else if(data.objetosComprados[i] == 2){
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setFichaComprada(); 
          }
        }
        
        //Actualizo la variable mapaSeleccionado de las variables de entorno
        if(data.mapaSel == 0){
          transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setmapaSeleccionado(false);
        }

        else{
          transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setmapaSeleccionado(true);
        }

        //Actualizo la variable FichaSeleccionada de las variables de entorno
        if(data.fichaSel == 0){
          transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setFichaSeleccionada(false);
        }

        else{
          transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setFichaSeleccionada(true);
        }

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

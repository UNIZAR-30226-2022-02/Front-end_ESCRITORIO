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
    public List<int> objetosComprados;
  }

  private screenManager sm;
  private VariablesEntorno varEntorno;

  void Start(){
    botonLogin.onClick.AddListener(loginAsync);    
    botonRegistro.onClick.AddListener(irRegistro); 

    sm = transform.parent.parent.GetComponent<screenManager>();
    varEntorno = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>();
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
        varEntorno.myUsername = username;

        //////////////////////////////////////////////////////////
        //LLamar al servidor para actualizar los datos del usuario
        WWWForm form2 = new WWWForm();
        form2.AddField("username", username);

        UnityWebRequest respuesta = UnityWebRequest.Post("serverrisk.herokuapp.com/login/datos", form2);
        yield return respuesta.Send();

        string resultado = respuesta.downloadHandler.text;
        Datos data = JsonUtility.FromJson<Datos>(resultado);

        // Puebla variables de entorno
        poblarEntorno(data);
       
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

  private void poblarEntorno(Datos data){
        if(data.enPartida != -1){
          varEntorno.estoyEnPartida = true;
        }
        else{
          varEntorno.estoyEnPartida = false;
        }

        //Actualizo los objetos comprados 
        foreach(int objeto in data.objetosComprados){
          if(objeto == 1){
            varEntorno.mapaComprado = true;
          }
          else if(objeto == 2){
            varEntorno.fichaComprada = true; 
          }
        }

        //Actualizo la variable mapaSeleccionado
        if(data.mapaSel == 0){
          varEntorno.mapaSeleccionado = false;
        }
        else{
          varEntorno.mapaSeleccionado = true;
        }

        //Actualizo la variable FichaSeleccionada 
        if(data.fichaSel == 0){
          varEntorno.fichaSeleccionada = false;
        }
        else{
          varEntorno.fichaSeleccionada = true;
        }
  }

  private void irRegistro()
  {
    sm.switchScreens(this.name, "Registro");
  }

  
}

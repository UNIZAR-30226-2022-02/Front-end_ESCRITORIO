using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class registroScript : MonoBehaviour
{

    public InputField mailInput, passwordInput, repeatPasswordInput,userInput;
    public Button botonRegistro, botonBack;
    public Text errorContrasena, errorUsuario;
    private screenManager sm;

    // Start is called before the first frame update
    void Start()
    {
        botonRegistro.onClick.AddListener(registro);
        botonBack.onClick.AddListener(irLogin);

        sm = transform.parent.parent.GetComponent<screenManager>();
    }

  private void registro()
  {
    String mail = mailInput.text;
    String pass =  passwordInput.text;
    String repeatPass =  repeatPasswordInput.text;
    String user =  userInput.text;

    StartCoroutine(envioRegistro(mail,pass,repeatPass,user));    
  }

  private IEnumerator envioRegistro(string mail, string pass, string repeatPass, string user)
  {
    WWWForm form = new WWWForm();
    form.AddField("username", user);
    form.AddField("password", pass);
    form.AddField("repeatPassword",repeatPass);
    form.AddField("mail", mail);
    UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/registro", form);

    yield return req.Send();

    if (req.error == "Contraseñas no coinciden")
    {
      //Quito el otro mensaje de error si está visible
      errorUsuario.gameObject.SetActive(false);
      errorContrasena.gameObject.SetActive(true);
    }
    else if(req.error == "Nombre de usuario no disponible"){
      //Quito el otro mensaje de error si está visible
      errorContrasena.gameObject.SetActive(false);
      errorUsuario.gameObject.SetActive(true);
    }
    else{

      //Guardo en el fichero de varaibles globales el nombre de usuario que se ha introducido
      transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().setUsername(user);

       //Si hay algun mensaje de error visible lo quito y me voy al home
      errorContrasena.gameObject.SetActive(false);
      errorUsuario.gameObject.SetActive(false);

      sm.switchScreens(this.name, "home");
    }
  }

  private void irLogin(){
    sm.switchScreens(this.name, "Login");
  }
}

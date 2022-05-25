using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class historialScript : MonoBehaviour
{
    public Button botonBack;
    public Text username;
    private screenManager sm;
    
    void Start()
    {
        //Recupero los datos del servidor
        /*UnityWebRequest respuesta = UnityWebRequest.Get("http://www.myserver.com/foo.txt");
        username.text = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().myUsername;
        //Hago desaparecer las 6 - numPartidas ultimas partidas (si num partidas < 5)
        if(respuesta.numPartidas < 5){
            for (int i = respuesta.numPartidas; i < 5; i++){
                Gameobject partidaFind = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).tranform.GetChild(0).tranform.GetChild(i).gameObject;
                objetoFind.SetActive(false);   
            }
        }
        
        //Muestro los numJugadores de cada partida
        for(int i = 0; i < respuesta.numPartidas; i++){
            for(int j = respuesta.jugadores[i]; j < 6; j++){
                GameObject jugadorFind = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).tranform.GetChild(0).tranform.GetChild(i).transform.GetChild(0).transform.GetChild(j);
                jugadorFind.SetActive(false);
            }
        }
       
        */
        botonBack.onClick.AddListener(irHome);
        sm = transform.parent.parent.GetComponent<screenManager>();

    }

    private void irHome()
    {
        //Pido los datos al servidor sobre el usuario
        /*WWWForm form = new WWWForm();
        string user = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().myUsername;
        form.AddField("username", user);
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/login/items", form);

        yield return req.Send();
        
        
        */



        sm.switchScreens(this.name, "Home");
    }


}

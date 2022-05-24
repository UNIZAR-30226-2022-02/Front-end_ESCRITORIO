using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
/*Al entrar a la pantalla recibir lo que tiene comprado y seleccionado (booleanos)
Cuando compro: enviar nombre de usuario, articulo comprado, y recibir si se ha podido comprar
Cuando selecciono: enviar nombre de usuario, articulo seleccionado (no recibo nada).
*/


public class tiendaScript : MonoBehaviour
{
    private screenManager sm;
    public Button botonBack, botonComprarMapa, botonComprarFicha, botonSeleccionarMapa, botonSeleccionarFicha, botonDeseleccionarMapa, botonDeseleccionarFicha;
    public bool fichaComprado, mapaComprado, fichaSeleccionado, mapaSeleccionado;
    public Text username, errorCompraMapa, errorCompraFicha;

    void Start()
    {
        //VariablesEntorno variablesEntorno = GetComponent<VariablesEntorno>();
        username.text = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().myUsername;
        
        //Creo los eventos de los botones
        
        botonBack.onClick.AddListener(irHome);

        botonComprarMapa.onClick.AddListener(ActivarComprarMapa);
        botonComprarFicha.onClick.AddListener(ActivarComprarFicha);

        botonSeleccionarMapa.onClick.AddListener(ActivarSeleccionarMapa);
        botonSeleccionarFicha.onClick.AddListener(ActivarSeleccionarFicha);

        botonDeseleccionarMapa.onClick.AddListener(QuitarSeleccionMapa);
        botonDeseleccionarFicha.onClick.AddListener(QuitarSeleccionFicha);

        sm = transform.parent.parent.GetComponent<screenManager>();

        //Para inicializar la pantalla si hay algo comprado o seleccionado
        if (fichaComprado == true){
            ActivarComprarFicha();
             Debug.Log ("ficha Comprada");
            if(fichaSeleccionado == true){
                ActivarSeleccionarFicha();
            }
        }

        if (mapaComprado == true){
            ActivarSeleccionarMapa();
            Debug.Log ("mapa Comprado");
            if(mapaSeleccionado == true){
                ActivarSeleccionarMapa();
            }
        }
    }

    private void irHome()
    {
        sm.switchScreens(this.name, "Home");
    }

    private void ActivarComprarMapa()
    {
        //Empiezo corutina
        StartCoroutine(compraObjeto("mapa"));

        botonComprarMapa.gameObject.SetActive(false);
        botonSeleccionarMapa.gameObject.SetActive(true);
        mapaComprado = true;
    }

    private void ActivarSeleccionarMapa()
    {
        //Empiezo corutina
        StartCoroutine(seleccionObjeto("mapa"));

        botonSeleccionarMapa.gameObject.SetActive(false);
        botonDeseleccionarMapa.gameObject.SetActive(true);
        mapaSeleccionado = true;
    }

    private void QuitarSeleccionMapa()
    {
        //Empiezo corutina
        StartCoroutine(QuitarSeleccionObjeto("mapa"));

        botonSeleccionarMapa.gameObject.SetActive(true);
        botonDeseleccionarMapa.gameObject.SetActive(false);
        mapaSeleccionado = false;
    }

    private void ActivarComprarFicha()
    {
        //Empiezo corutina
        StartCoroutine(compraObjeto("ficha"));

        botonComprarFicha.gameObject.SetActive(false);
        botonSeleccionarFicha.gameObject.SetActive(true);
        fichaComprado = true;
    }

    private void ActivarSeleccionarFicha()
    {
        //Empiezo corutina
        StartCoroutine(seleccionObjeto("ficha"));

        botonSeleccionarFicha.gameObject.SetActive(false);
        botonDeseleccionarFicha.gameObject.SetActive(true);
        fichaSeleccionado = true;
    }

    private void QuitarSeleccionFicha()
    {
        //Empiezo corutina
        StartCoroutine(QuitarSeleccionObjeto("ficha"));

        botonSeleccionarFicha.gameObject.SetActive(true);
        botonDeseleccionarFicha.gameObject.SetActive(false);
        fichaSeleccionado = false;
    }

    private IEnumerator compraObjeto(string objetoComprado)
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        if (objetoComprado == "mapa"){
            form.AddField("compradoMapa","true");
        }
        else{
            //Ver el formato de los mensajes
            form.AddField("compradoFicha","true");    
        }

        //Envio el mensaje
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/tienda", form);
        yield return req.Send();

        //La compra no se ha podido realizar y muestro los mensajes de error
        if (req.error != null & objetoComprado == "mapa"){
            errorCompraMapa.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorCompraMapa.gameObject.SetActive(false);
        }
        else if(req.error != null & objetoComprado == "ficha"){
            errorCompraFicha.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorCompraFicha.gameObject.SetActive(false);
        }
        //La compra se ha podido realizar
        else{
            errorCompraMapa.gameObject.SetActive(false);
            errorCompraFicha.gameObject.SetActive(false);
        }    
    }

    private IEnumerator seleccionObjeto(string objetoSeleccionado)
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        //Compruebo que objeto he seleccionado
        if (objetoSeleccionado == "mapa"){
            form.AddField("seleccionadoMapa","true");
        }
        else{
            //Ver el formato de los mensajes
            form.AddField("seleccionadoFicha","true");    
        }

        //Envio el mensaje y no recibo nada
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/tienda", form);
        yield return null;
    }

    private IEnumerator QuitarSeleccionObjeto(string objetoSeleccionado){
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        //Compruebo que objeto he seleccionado
        if (objetoSeleccionado == "mapa"){
            form.AddField("seleccionadoMapa","false");
        }
        else{
            //Ver el formato de los mensajes
            form.AddField("seleccionadoFicha","false");    
        }

        //Envio el mensaje y no recibo nada
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/tienda", form);
        yield return null;
    }
}

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
    public Text username, errorCompraMapa, errorCompraFicha;
    
    void Start()
    {
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

    }

    private void irHome()
    {
        
        sm.switchScreens(this.name, "Home");
    }

    private void ActivarComprarMapa()
    {
        //Empiezo corutina
        StartCoroutine(compraObjeto("mapa"));
    }

    private void ActivarSeleccionarMapa()
    {
        //Empiezo corutina
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenMapaSeleccionado.gameObject.SetActive(true);
        StartCoroutine(seleccionMapa());
    }

    private void QuitarSeleccionMapa()
    {
        //Empiezo corutina
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenMapaSeleccionado.gameObject.SetActive(false);
        StartCoroutine(QuitarSeleccionObjeto("mapa"));
    }

    private void ActivarComprarFicha()
    {
        //Empiezo corutina
        StartCoroutine(compraObjeto("ficha"));   
    }

    private void ActivarSeleccionarFicha()
    {
        //Empiezo corutina
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenFichaSeleccionada.gameObject.SetActive(true);
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenFichaPredeterminada.gameObject.SetActive(false);
        StartCoroutine(seleccionFicha());
    }

    private void QuitarSeleccionFicha()
    {
        //Empiezo corutina
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenFichaSeleccionada.gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.GetComponent<homeScript>().imagenFichaPredeterminada.gameObject.SetActive(true);
        StartCoroutine(QuitarSeleccionObjeto("ficha"));
    }

    private IEnumerator compraObjeto(string objetoComprado)
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        if (objetoComprado == "mapa"){
            form.AddField("item","1");
        }
        else{
            form.AddField("item","2");    
        }

        //Envio el mensaje
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/tienda/comprar", form);
        yield return req.Send();

        string res = req.downloadHandler.text;

        Debug.Log("Recibido resultado compra " + objetoComprado + " " + res);
        //La compra no se ha podido realizar y muestro los mensajes de error
        if (res == "false" & objetoComprado == "mapa"){
            errorCompraMapa.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorCompraMapa.gameObject.SetActive(false);
            Debug.Log("No se ha podido comprar el mapa");
        }

        else if(res == "false" & objetoComprado == "ficha"){
            errorCompraFicha.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorCompraFicha.gameObject.SetActive(false);
            Debug.Log("No se ha podido comprar la ficha");
        }

        //La compra se ha podido realizar
        else if(res == "true" & objetoComprado == "mapa"){
            Debug.Log("Se ha podido realizar la compra de mapa");
            errorCompraMapa.gameObject.SetActive(false);
            errorCompraFicha.gameObject.SetActive(false);
            botonComprarMapa.gameObject.SetActive(false);
            botonSeleccionarMapa.gameObject.SetActive(true);
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().mapaComprado = true;
        } 

        else if(res == "true" & objetoComprado == "ficha"){
            Debug.Log("Se ha podido realizar la compra de ficha");
            errorCompraMapa.gameObject.SetActive(false);
            errorCompraFicha.gameObject.SetActive(false);
            botonComprarFicha.gameObject.SetActive(false);
            botonSeleccionarFicha.gameObject.SetActive(true);
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().fichaComprada = true;
        } 
    }

    private IEnumerator seleccionMapa()
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);
    
        //Compruebo que objeto he seleccionado
        form.AddField("mapa","1");
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/store/selectMapa", form);
        transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().mapaSeleccionado = true;
        botonSeleccionarMapa.gameObject.SetActive(false);
        botonDeseleccionarMapa.gameObject.SetActive(true);
        yield return req.Send();
    }

    private IEnumerator seleccionFicha()
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);
    
        //Ver el formato de los mensajes
        form.AddField("fichas","2");    
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/store/selectFichas", form);
        Debug.Log("Enviado seleccionado fichas");
        yield return req.Send();

        botonSeleccionarFicha.gameObject.SetActive(false);
        botonDeseleccionarFicha.gameObject.SetActive(true);
        transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().fichaSeleccionada = true;

    }

    private IEnumerator QuitarSeleccionObjeto(string objetoSeleccionado){
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        //Compruebo que objeto he seleccionado
        if (objetoSeleccionado == "mapa"){
            form.AddField("mapa","0");   
            botonSeleccionarMapa.gameObject.SetActive(true);
            botonDeseleccionarMapa.gameObject.SetActive(false); 
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().mapaSeleccionado = false;
        }
        else{
            //Ver el formato de los mensajes
            form.AddField("fichas","0");
            botonSeleccionarFicha.gameObject.SetActive(true);
            botonDeseleccionarFicha.gameObject.SetActive(false);
            transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().fichaSeleccionada = false;    
        }

        //Envio el mensaje y no recibo nada
        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/tienda", form);
        Debug.Log("Enviado DESseleccionado " + objetoSeleccionado);
        yield return req.Send();
    }
}

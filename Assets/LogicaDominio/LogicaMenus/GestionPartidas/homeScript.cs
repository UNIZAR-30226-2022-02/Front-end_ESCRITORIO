using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class homeScript : MonoBehaviour
{
    public Button botonTienda, botonHistorial, botonCrearPartidaDatos,botonCrearPartidaEnviar, botonBuscarPartida, botonUnirse, botonAtras;
    public Text username,errorBuscarPartida, errorUnirsePartida, errorEstoyEnPartida;
    public InputField codigoPartida;
    public Dropdown tipoSincronizacion, tipoPrivacidad, numJugadoresCrear, numJugadoresBuscar;
    public GameObject BordeCrearPartida,BordeEsperarJugadores;
    private screenManager sm;
    // Start is called before the first frame update
    void Start()
    {
        username.text = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().myUsername;
        
        bool enPartida = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().estoyEnPartida;

        if(enPartida){
            //PEDIR JUGADAS Y METERLAS A LA COLA
            /*
            //Desactivo 
            List<Jugadas> jugadas...

            foreach(Jugada jug in jugadas){
                transform.parent.parent.gameObject.GetComponent<ColaJugadas>().nuevaJugada(jug);
            }
            */
        }

        botonAtras.onClick.AddListener(quitarCrearPartida);
        botonTienda.onClick.AddListener(tienda);
        botonHistorial.onClick.AddListener(historial);
        botonCrearPartidaDatos.onClick.AddListener(crearPartida);
        botonCrearPartidaEnviar.onClick.AddListener(enviarDatosCrearPartida);
        botonBuscarPartida.onClick.AddListener(buscarPartida);
        botonUnirse.onClick.AddListener(unirsePartida);
        
        sm = transform.parent.parent.GetComponent<screenManager>();
    }

    //Muestro los posibles mensajes de error del home durante dos segundos
    private IEnumerator MostrarError(string mensajeError)
    {
        if(mensajeError == "estoyEnPartida"){
            errorEstoyEnPartida.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorEstoyEnPartida.gameObject.SetActive(true);
        }
        else if(mensajeError == "errorUnirse"){
            errorUnirsePartida.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorUnirsePartida.gameObject.SetActive(false);
        }
        else if(mensajeError == "errorBuscar"){
            errorBuscarPartida.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            errorBuscarPartida.gameObject.SetActive(false);
        }
    }
    

    //Cambio a pantalla tienda
    private void tienda()
    {
        //Para inicializar la tienda si la ficha esta comprada o seleccionada
        if(transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().fichaComprada == true){
            transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonComprarFicha.gameObject.SetActive(false);
            transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonSeleccionarFicha.gameObject.SetActive(true);
            if(transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().fichaSeleccionada == true){
                transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonSeleccionarFicha.gameObject.SetActive(false);
                transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonDeseleccionarFicha.gameObject.SetActive(true);
            }
        }

        //Para inicializar la tienda si el mappa esta comprado o seleccionado
        if (transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().mapaComprado == true){
            transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonComprarMapa.gameObject.SetActive(false);
            transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonSeleccionarMapa.gameObject.SetActive(true);
            if(transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().mapaSeleccionado == true){
                transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonSeleccionarMapa.gameObject.SetActive(false);
                transform.parent.GetChild(4).gameObject.GetComponent<tiendaScript>().botonDeseleccionarMapa.gameObject.SetActive(true);
            }
        }
        sm.switchScreens(this.name, "Tienda");
    }

    //Cambio a pantalla historial
    private void historial()
    {
        sm.switchScreens(this.name, "Historial");
    }

    //Muestro el pop up de crear partida
    private void crearPartida()
    {
        BordeCrearPartida.SetActive(true);
    }

    //Quito el pop up de crear una partida
    private void quitarCrearPartida()
    {
        BordeCrearPartida.SetActive(false);
    }

    //LLamo a una corutina para enviar los datos introducidos en el pop up de crear partida 
    private void enviarDatosCrearPartida(){
        StartCoroutine(enviarDatosCrear());
    }

    //Envio los datos necesarios para crear una partida y no recibo nada (supongo que siempre se puede)
    private IEnumerator enviarDatosCrear()
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        string privacidad = tipoPrivacidad.captionText.text;
        form.AddField("publica", privacidad);

        string sincronizacion = tipoSincronizacion.captionText.text;
        form.AddField("tipo", sincronizacion);

        int numJugadoresCrearPartida = numJugadoresCrear.value + 2;
        form.AddField("maxJugadores",numJugadoresCrearPartida);

        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/partida/crearPartida", form);

        yield return req.Send();

        BordeEsperarJugadores.gameObject.SetActive(true);
    }

    //Veo si estoy en una partida, si lo estoy muestro mensaje de error, sino llamo al servidor para
    //que busque una partida
    private void buscarPartida()
    {
        bool enPartida = transform.parent.parent.gameObject.GetComponent<VariablesEntorno>().estoyEnPartida;
        //Si estoy en partida muestro error
        if(enPartida){
            StartCoroutine(MostrarError("estoyEnPartida"));
        }
        //Sino le envio datos al servidor para que busque una partida
        else{
            StartCoroutine(enviarDatosBuscar());
        } 
    }

    //Envio los datos al servidor para que busque una partida
    private IEnumerator enviarDatosBuscar()
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        form.AddField("publica","Publica");

        string numJugadores = numJugadoresBuscar.captionText.text;
        form.AddField("numJugadores", numJugadores);

        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/partida/unirPartida", form);

        yield return req.Send();
        string resultado = req.downloadHandler.text;

        if(req.error != null){
            if(resultado != "OK"){
                StartCoroutine(MostrarError("errorBuscar"));
            }
            else{
                BordeEsperarJugadores.gameObject.SetActive(true);
            }
        }
            
    }

    //LLamo a una corutina para que envie al servidor los datos para unirse a una partida
    private void unirsePartida()
    {
        StartCoroutine(enviarDatosUnirse());
    }

    //Envio los datos necesarios al servidor un codigo de partida y un nombre de usuario,
    //recibo si hay alguna partida a la que me puedo unir
    private IEnumerator enviarDatosUnirse()
    {
        WWWForm form = new WWWForm();
        string user = username.text;
        form.AddField("username", user);

        form.AddField("publica","Privada");

        string codigo = codigoPartida.text;
        form.AddField("codigoPartida", codigo);

        UnityWebRequest req = UnityWebRequest.Post("serverrisk.herokuapp.com/partida/unirPartida", form);

        yield return req.Send();

        string resultado = req.downloadHandler.text;
        if(req.error != null){
            if(resultado != "OK"){
                StartCoroutine(MostrarError("errorUnirse"));
            }      
        }
        else{
            Debug.Log("error: " + req.error);
        }
    }
}

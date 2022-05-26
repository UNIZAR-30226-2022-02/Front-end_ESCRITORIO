using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LogicaInGame.Jugadas;

public class ElegirNumeroTropas : MonoBehaviour
{

    Partida myGame;
    WebSocketHandler wsHandler;

    public Text prompt, valorSlider;
    public Button botonCerrar, botonConfirmar;
    public Slider slider;

    //Varaibles temporales
    Jugador jugador;
    Territorio terrOrig, terrDest;

    Accion accion;
    private enum Accion{
        poner,
        mover,
        atacar,
        defender,
        nula
    };

    // ====================
    // - Metodos Publicos -
    // ====================
    public void mostrarPonerTropas(Territorio t, Jugador j){
        accion = Accion.poner;
        jugador = j;
        terrDest = t;

        prompt.text = "Elige el numero de tropas que quieres colocar...";
        slider.maxValue = j.getNTropasSinColocar();

        this.gameObject.SetActive(true);
    }

    public void mostrarMoverTropas(Territorio tOrig, Territorio tDest, Jugador j){
        accion = Accion.mover;
        jugador = j;
        terrOrig = tOrig;
        terrDest = tDest;

        prompt.text = "Elige el numero de tropas que quieres mover...";
        slider.maxValue = tOrig.getNumTropas()-1;

        this.gameObject.SetActive(true);
    }

    public void mostrarAtacar(Territorio t, Jugador j){
    }

    // =====================================0

    void Start()
    {
        myGame = this.transform.parent.gameObject.GetComponent<Partida>();
        wsHandler = this.transform.parent.parent.gameObject.GetComponent<WebSocketHandler>();

        botonCerrar.onClick.AddListener(cerrarVentana);
        botonConfirmar.onClick.AddListener(realizaJugada);
        slider.onValueChanged.AddListener(delegate {actualizaValor();});

        accion = Accion.nula;
        jugador = null;
        terrOrig = null;
        terrDest = null;

        slider.minValue = 1;

    }


    private void cerrarVentana(){
        accion = Accion.nula;
        this.gameObject.SetActive(false);
    }

    void actualizaValor(){
        valorSlider.text = slider.value.ToString();
    }

    private void realizaJugada(){
        switch (accion){
            case Accion.poner:
                Jugada j = new JugadaPonerTropas(jugador.id, myGame.idPartida, terrDest.id, (int) slider.value);
                wsHandler.notificaJugada(j);
                break;

            case Accion.mover:
                j = new JugadaMoverTropas(jugador.id, myGame.idPartida, terrOrig.id, terrDest.id, (int) slider.value);
                wsHandler.notificaJugada(j);

                // Fin turno
                wsHandler.notificaJugada(new JugadaFinTurno(jugador.id, myGame.idPartida));
                break;

            case Accion.atacar:
                break;
            case Accion.defender:
                break;

        }

        this.gameObject.SetActive(false);
    }

    
}

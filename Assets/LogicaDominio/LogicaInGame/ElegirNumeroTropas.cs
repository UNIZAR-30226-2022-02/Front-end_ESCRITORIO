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
        this.gameObject.SetActive(true);
        botonCerrar.gameObject.SetActive(true);

        jugador = j;
        terrDest = t;

        accion = Accion.poner;
        prompt.text = "Elige el numero de tropas que quieres colocar...";
        slider.maxValue = j.getNTropasSinColocar();
    }

    public void mostrarMoverTropas(Territorio tOrig, Territorio tDest, Jugador j){
        this.gameObject.SetActive(true);
        botonCerrar.gameObject.SetActive(true);

        jugador = j;
        terrOrig = tOrig;
        terrDest = tDest;

        accion = Accion.mover;
        prompt.text = "Elige el numero de tropas que quieres mover...";
        Debug.Log("ElegirNumTropas: TOrig=" + tOrig);
        slider.maxValue = tOrig.getNumTropas()-1;
    }

    public void mostrarAtacar(Territorio tOrig, Territorio tDest, Jugador j){
        this.gameObject.SetActive(true);
        botonCerrar.gameObject.SetActive(true);


        jugador = j;
        terrOrig = tOrig;
        terrDest = tDest;

        accion = Accion.atacar;
        prompt.text = "Elige el numero de tropas con las que quieres atacar...";

        int nDados= Mathf.Min(tOrig.getNumTropas()-1, 3);
        slider.maxValue = nDados;
    }

    public void mostrarDefender(Territorio tOrig, Territorio tDest, Jugador j){
        this.gameObject.SetActive(true);
        botonCerrar.gameObject.SetActive(false);

        jugador = j;
        terrOrig = tOrig;
        terrDest = tDest;

        accion = Accion.defender;
        prompt.text = tOrig.id +  " atacando " + tDest.id + ": ¿Con cuantas tropas defiendes?";

        int nDados= Mathf.Min(tDest.getNumTropas(), 2);
        slider.maxValue = nDados;
    }



    // =====================================0

    void Start()
    {
        myGame = this.transform.parent.gameObject.GetComponent<Partida>();
        wsHandler = this.transform.parent.parent.gameObject.GetComponent<WebSocketHandler>();

        botonCerrar.onClick.AddListener(cerrarVentana);
        botonConfirmar.onClick.AddListener(realizaJugada);
        slider.onValueChanged.AddListener(delegate {actualizaValor();});

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
        Debug.Log("PopUpNumTropas boton pulsado..., accion = " + accion);
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
                if(myGame.partidaSincrona){
                    int[] dadosAtaque = tirarDados((int) slider.value);
                    j = new JugadaAtaqueSincrono(jugador.id, myGame.idPartida, terrOrig.id, terrDest.id, dadosAtaque);

                }
                else{
                    int[] dadosAtaque = tirarDados((int) slider.value);
                    
                    int nDadosDef = Mathf.Min(2, terrDest.getNumTropas());
                    int[] dadosDef = tirarDados(nDadosDef);

                    j = new JugadaAtaqueAsincrono(jugador.id, myGame.idPartida, terrOrig.id, terrDest.id, dadosAtaque, dadosDef);
                }
                wsHandler.notificaJugada(j);
                break;

            case Accion.defender:
                int[] dadosDefensa = tirarDados((int) slider.value);
                j = new JugadaDefensaSincrona(jugador.id, myGame.idPartida, terrOrig.id, terrDest.id, dadosDefensa);
                wsHandler.notificaJugada(j);
                break;

        }
        
        this.gameObject.SetActive(false);
    }

    private int[] tirarDados(int nDados){
        int[] res = new int[nDados];

        for(int i=0; i<nDados; i++){
            res[i] = Random.Range(1,6);
        }
        return res;
    }
    
}

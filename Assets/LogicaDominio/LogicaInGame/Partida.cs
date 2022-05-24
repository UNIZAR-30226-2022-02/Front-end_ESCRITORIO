using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogicaInGame.Jugadas;

public class Partida : MonoBehaviour
{    
    WebSocketHandler wsHandler;
    ColaJugadas jugadas;

    // Info Partida
    public int idPartida {get; set;}
    public bool partidaSincrona { get; set;}
    int nVecesCartasUsadas { get; set;}

    // Jugadores
    public int myId {get; set;} // TODO: inicializar valor con variable global (JUAN)
    public List<Jugador> jugadores;
    public int nJugadores {get; set;}

    // Turno
    Turno turno;

    // Territorios
    List<Territorio> territorios;

    // Jugadores eliminados
    int nJugadoresEliminados;
    public List<int> jugadoresEliminados {get; set;}

    // Variables temporales
    public Territorio attackingFrom; // Solo tiene valor en la fase de ataque del turno
    public Territorio movingFrom; // Solo tiene valor en la fase de ataque del turno

    // GUI
    private Text MensajeError;

    // ====================
    // - Metodos Publicos -
    // ====================

    // Devuelve true si todos los territorios tienen un
    // propietario, falso si no
    public bool todosTerritoriosConquistados(){
        foreach (Territorio t in territorios){
            if (t.getPropietario() == -1){
                return false;
            }
        }
        return true;
    }

    public IEnumerator ShowError(string msg, int timeSeconds){
        MensajeError.text = msg;
        MensajeError.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeSeconds);

        MensajeError.gameObject.SetActive(false);
    }

    // ===========================================================================
    
    void Start()
    {
        wsHandler = this.transform.parent.GetComponent<WebSocketHandler>();
        jugadas = this.transform.parent.GetComponent<ColaJugadas>();

        // Info partida
        nVecesCartasUsadas = 0;

        nJugadores = 0;

        turno = this.transform.Find("Turno").gameObject.GetComponent<Turno>();

        nJugadoresEliminados = 0;
        jugadoresEliminados = new List<int>();

        attackingFrom = null;
        movingFrom = null;
    }

    void Update(){
        while(jugadas.hayJugadas()){
            procesarJugada(jugadas.ultimaJugada());
        }
    }

    // ============================
    // - PROCESAMIENTO DE JUGADAS -
    // ============================
    private void procesarJugada(Jugada j){
        //Debug.Log("Procesando jugada...");
        switch (j.type){
            case "crearPartida":
                crearPartida( (JugadaCrearPartida) j);
                break;
            case "finTurno":
                finTurno( (JugadaFinTurno) j);
                break;
            
            case "ponerTropas":
                ponerTropas((JugadaPonerTropas) j);
                break;
            case "moverTropas":
                moverTropas((JugadaMoverTropas) j);
                break;
            /*
            case "utilizarCartas":
                utilizarCartas((JugadaUtilizarCartas) j);
                break;
            case "ataqueSincrono":
                ataqueSincrono((JugadaAtaqueSincrono) j);
                break;
            case "defensaSincrona":
                defensaSincrona((JugadaDefensaSincrona) j);
                break;
            case "ataqueAsincrono":
                ataqueAsincrono((JugadaAtaqueAsincrono) j);
                break;
            case "pedirCarta":
               pedirCarta((JugadaPedirCarta) j);
               break;
            case "finPartida":
                finPartida((JugadaFinPartida) j);
                break;  
            */          
            default:
                Debug.Log("Error: " + j.type + " no es un tipo de jugada valida.");
                break;
        }
    }

    private void crearPartida( JugadaCrearPartida j){
        Debug.Log("Creando partida... Jugada: " + j.ToString());
        
        idPartida = j.idPartida;    
        partidaSincrona = j.partidaSincrona;

        // Inicializo jugadores
        nJugadores = j.listaJugadores.Length;
        for (int i=0; i<nJugadores; i++){
            jugadores[i].inicializa(j.listaJugadores[i], nJugadores);

            //Inicializo myId
            myId = 0;  // DEBUG!
            /*
            VariablesEntorno ve = this.transform.parent.gameObject.GetComponent<VariablesEntorno>();
            if(j.listaJugadores[i] == ve.myUsername){
                myId = i;
            }
            */
        }

        // Oculto jugadores sobrantes
        for (int i=nJugadores; i<jugadores.Count; i++){
            jugadores[i].gameObject.SetActive(false);
        }

    }

    private void finTurno(JugadaFinTurno j){
        if(turno.checkTurno(j)){
            turno.avanzaTurno();
            attackingFrom = null;
            movingFrom = null;
        }
    }

    
    private void ponerTropas(JugadaPonerTropas j){
        if(turno.checkTurno(j)){
            Territorio t = territorios.Find(aux => aux.id == j.idTerritorio);
            Jugador pl =  jugadores[j.userId];
            if (!todosTerritoriosConquistados() || t.getPropietario()!=j.userId){
                Debug.Log("Error en ponerTropas: El jugador no es propietario del territorio.");
                return;
            } 
            if (pl.getNTropasSinColocar()<j.numTropas){
                Debug.Log("Error en ponerTropas: El jugador no tiene suficientes tropas.");
                return;
            } 

            t.setPropietario(myId);
            int nuevoNumTropas = t.getNumTropas() + j.numTropas;
            t.setNumTropas(nuevoNumTropas);
            pl.setNTropasSinColocar(pl.getNTropasSinColocar()-j.numTropas);
        }
    }

    
    private void moverTropas(JugadaMoverTropas j){
        if(turno.checkTurno(j)){
            Territorio tOrig = territorios.Find(aux => aux.id == j.idTerritorioOrigen);
            Territorio tDest = territorios.Find(aux => aux.id == j.idTerritorioDestino);
            Jugador pl =  jugadores[j.userId];

            if (tOrig.getPropietario()!=j.userId){
                Debug.Log("Error en moverTropas: El jugador no es propietario del pais de origen.");
                return;
            } 
            if (tDest.getPropietario()!=j.userId){
                Debug.Log("Error en moverTropas: El jugador no es propietario del pais de destino.");
                return;
            } 
            if (tOrig.getNumTropas() < j.numTropas+1){
                Debug.Log("Error en moverTropas: El territorio origen no tiene suficientes tropas.");
                return;
            } 

            int nuevoNumTropas = tOrig.getNumTropas() - j.numTropas;
            tOrig.setNumTropas(nuevoNumTropas);

            nuevoNumTropas = tDest.getNumTropas() + j.numTropas;
            tDest.setNumTropas(nuevoNumTropas);
        }
    }


    /*
    private void utilizarCartas( JugadaUtilizarCartas j){

    }

    private void ataqueSincrono(JugadaAtaqueSincrono j){

    }

    private void defensaSincrona(JugadaDefensaSincrona j){

    }

    private void ataqueAsincrono(JugadaAtaqueAsincrono j){

    }
    
    private void pedirCarta(JugadaPedirCarta j){

    }

    private void finPartida(JugadaFinPartida j){

    }*/

}


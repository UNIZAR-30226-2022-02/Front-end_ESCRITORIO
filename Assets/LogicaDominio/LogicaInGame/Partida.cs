using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogicaInGame.Jugadas;

public class Partida : MonoBehaviour
{    
    WebSocketHandler wsHandler;
    ColaJugadas jugadas;
    VariablesEntorno entorno;
    ElegirNumeroTropas popUpNumTropas;

    public GameObject fondoDados;
    public Dado[] dadosAtaque;
    public Dado[] dadosDefensa;

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
    public List<Territorio> territorios;

    // Jugadores eliminados
    int nJugadoresEliminados;
    public List<int> jugadoresEliminados {get; set;}

    // Variables temporales
    public Territorio attackingFrom {get; set;} // Solo tiene valor en la fase de ataque del turno
    public Territorio movingFrom {get; set;} // Solo tiene valor en la fase de ataque del turno
    
    public Jugada ultJugada {get; set;}

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
        wsHandler = this.transform.parent.gameObject.GetComponent<WebSocketHandler>();
        jugadas = this.transform.parent.gameObject.GetComponent<ColaJugadas>();
        entorno =  this.transform.parent.gameObject.GetComponent<VariablesEntorno>();
        popUpNumTropas = this.transform.Find("PopUpNumTropas").gameObject.GetComponent<ElegirNumeroTropas>();

        // Info partida
        nVecesCartasUsadas = 0;

        nJugadores = 0;

        turno = this.transform.Find("Turno").gameObject.GetComponent<Turno>();
        
        nJugadoresEliminados = 0;
        jugadoresEliminados = new List<int>();

        attackingFrom = null;
        movingFrom = null;
        ultJugada = null;

        MensajeError = this.transform.Find("MensajeError").gameObject.GetComponent<Text>();
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
            default:
                Debug.Log("Error: " + j.type + " no es un tipo de jugada valida.");
                break;
        }
        ultJugada = j;
    }

    private void crearPartida( JugadaCrearPartida j){
        Debug.Log("Creando partida... Jugada: " + j.ToString());
        
        idPartida = j.idPartida;    
        partidaSincrona = j.partidaSincrona;

        // Inicializo jugadores
        nJugadores = j.listaJugadores.Length;
        Debug.Log("nJugadores = " + nJugadores);
        
        for (int i=0; i<nJugadores; i++){
            jugadores[i].inicializa(j.listaJugadores[i], nJugadores);

            //Inicializo myId            
            if(j.listaJugadores[i] == entorno.myUsername){
                myId = i;
            }
            
        }

        // Oculto jugadores sobrantes
        for (int i=nJugadores; i<jugadores.Count; i++){
            jugadores[i].gameObject.SetActive(false);
        }

    }

    private void finTurno(JugadaFinTurno j){
        Debug.Log("Finalizando turno... Jugada: " + j.ToString());

        if(turno.checkTurno(j)){
            turno.avanzaTurno();
            attackingFrom = null;
            movingFrom = null;
        }
    }

    
    private void ponerTropas(JugadaPonerTropas j){
        Debug.Log("Poniendo tropas... Jugada: " + j.ToString());

        if(turno.checkTurno(j)){
            Territorio t = territorios.Find(aux => aux.id == j.idTerritorio);
            Jugador pl =  jugadores[getIdJugador(j.userId)];
            if (todosTerritoriosConquistados() && t.getPropietario()!=getIdJugador(j.userId)){
                Debug.Log("Error en ponerTropas: El jugador no es propietario del territorio.");
                return;
            } 
            if (pl.getNTropasSinColocar()<j.numTropas){
                Debug.Log("Error en ponerTropas: El jugador no tiene suficientes tropas.");
                return;
            } 

            t.setPropietario(getIdJugador(j.userId));

            int nuevoNumTropas = t.getNumTropas() + j.numTropas;
            t.setNumTropas(nuevoNumTropas);
            pl.setNTropasSinColocar(pl.getNTropasSinColocar()-j.numTropas);
        }
    }

    
    private void moverTropas(JugadaMoverTropas j){
        Debug.Log("Moviendo tropas... Jugada: " + j.ToString());

        if(turno.checkTurno(j)){
            Territorio tOrig = territorios.Find(aux => aux.id == j.idTerritorioOrigen);
            Territorio tDest = territorios.Find(aux => aux.id == j.idTerritorioDestino);
            Debug.Log("moverTropas: tOrig = " + tOrig);
            Debug.Log("moverTropas: tDest = " + tDest);
            Jugador pl =  jugadores[getIdJugador(j.userId)];

            if (tOrig.getPropietario()!=getIdJugador(j.userId)){
                Debug.Log("Error en moverTropas: El jugador no es propietario del pais de origen.");
                return;
            } 
            if (tDest.getPropietario()!=getIdJugador(j.userId)){
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


    private void utilizarCartas( JugadaUtilizarCartas j){
        if(turno.checkTurno(j)){

        }
    }

    private void ataqueSincrono(JugadaAtaqueSincrono j){
        Debug.Log("Comienza ataque sincrono... Jugada: " + j.ToString());

        if (turno.checkTurno(j)){
            Territorio atacante =  territorios.Find(aux => aux.id == j.territorioAtacante);
            Territorio atacado =  territorios.Find(aux => aux.id == j.territorioAtacado);

            if(atacante.getPropietario() != getIdJugador(j.userId)){
                Debug.Log("Error en ataqueSincrono: El jugador no es propietario del territorio atacante.");
                return;
            }
            if(atacado.getPropietario() == getIdJugador(j.userId)){
                Debug.Log("Error en ataqueSincrono: El jugador no puede atacar su propio territorio.");
                return;
            }
            if(!atacante.TerrColindantes.Contains(atacado)){
                Debug.Log("Error en ataqueSincrono: Los territorios deben ser colindantes.");
                return;
            }

            if(atacado.getPropietario() == myId){
                popUpNumTropas.mostrarDefender(atacante, atacado, jugadores[myId]);
            }
        }
    }

    private void defensaSincrona(JugadaDefensaSincrona j){
        Debug.Log("Comienza defensa sincrona... Jugada: " + j.ToString());

        // Verifico que exista un ataque
        if(ultJugada.type!="ataqueSincrono"){
            Debug.Log("Error en defensaSincriona: La anterior jugada no es un ataque, es: " + j.type);
            return;
        }
        JugadaAtaqueSincrono ataque = (JugadaAtaqueSincrono) ultJugada;
        
        if(ataque.territorioAtacante != j.territorioAtacante || ataque.territorioAtacado != j.territorioAtacado){
            Debug.Log("Error en defensaSincriona: Los territorios del ataque y la defensa no coinciden.");
            return;
        }

        Territorio atacado =  territorios.Find(aux => aux.id == j.territorioAtacado);
        if(atacado.getPropietario() != getIdJugador(j.userId)){
            Debug.Log("Error en defensaSincriona: El jugador no es propietario del territorio atacado.");
            return;
        }
        
        StartCoroutine(decidirBatalla(ataque.resultadoDadosAtaque, j.resultadoDadosDefensa, ataque.territorioAtacante, ataque.territorioAtacado));
    }

    private void ataqueAsincrono(JugadaAtaqueAsincrono j){
        if(turno.checkTurno(j)){
            Territorio atacante =  territorios.Find(aux => aux.id == j.territorioAtacante);
            Territorio atacado =  territorios.Find(aux => aux.id == j.territorioAtacado);

            if(atacante.getPropietario() != getIdJugador(j.userId)){
                Debug.Log("Error en ataqueAsincrono: El jugador no es propietario del territorio atacante.");
                return;
            }
            if(atacado.getPropietario() == getIdJugador(j.userId)){
                Debug.Log("Error en ataqueAsincrono: El jugador no puede atacar su propio territorio.");
                return;
            }
            if(!atacante.TerrColindantes.Contains(atacado)){
                Debug.Log("Error en ataqueAsincrono: Los territorios deben ser colindantes.");
                return;
            }

            StartCoroutine(decidirBatalla(j.resultadoDadosAtaque, j.resultadoDadosDefensa, j.territorioAtacante, j.territorioAtacado));
        }
    }
    
    private void pedirCarta(JugadaPedirCarta j){
        if(turno.checkTurno(j)){
            jugadores[getIdJugador(j.userId)].cartas.addCarta(j.cartaRecibida); 
        }
    }

    private void finPartida(JugadaFinPartida j){

    }

    public int getIdJugador(String username){
        try{
            Jugador j = jugadores.Find(aux => aux.userName == username);
            return j.id;
        }
        catch (Exception e){
            Debug.Log("ERROR getIdJugador: El jugador "+ username + " no esta en la lista.\n" + e);
            return -1;
        }
    }

    // Funciones auxiliares
    private IEnumerator decidirBatalla(int[] resAtaque, int[] resDefensa, string idTerrAtaque, string idTerrDefensa){
        Debug.Log("Decidiendo batalla...");

        if(!jugadas.getRecuperandoEstado()){
            // Muestra dados tirados
            int j = 0;
            fondoDados.SetActive(true);
            foreach(int val in resAtaque){
                dadosAtaque[j++].mostrarTirada(val);
            }
            j = 0;
            foreach(int val in resDefensa){
                dadosDefensa[j++].mostrarTirada(val);
            }
            yield return new WaitForSeconds(Dado.duracionTirada);
            fondoDados.SetActive(false);
        }



        int[] atqAux = resAtaque;
        int[] defAux = resDefensa;
        Array.Sort(atqAux); Array.Reverse(atqAux);
        Array.Sort(defAux); Array.Reverse(defAux);

        Territorio tAtq = territorios.Find(aux => aux.id == idTerrAtaque);
        Territorio tDef = territorios.Find(aux => aux.id == idTerrDefensa);
        int jugAtq = tAtq.getPropietario();
        int jugDef = tDef.getPropietario();

        int n = Mathf.Min(atqAux.Length, defAux.Length);
        for(int i=0; i<n; i++){
            if(atqAux[i] > defAux[i]){
                // Gana atacante
                Debug.Log("Dado " + i + ": Gana atacante!");

                tDef.setNumTropas(tDef.getNumTropas()-1);

                
                if(tDef.sePuedeConquistar()){
                    // Consigue conquistar
                    StartCoroutine(ShowError("¡" + tDef.id + " conquistado por " + jugadores[jugAtq].userName +"!" , 5));

                    // Actualiza propietario
                    tDef.setPropietario(jugAtq);

                    // Mueve tropas
                    tDef.setNumTropas(resAtaque.Length);
                    tAtq.setNumTropas(tAtq.getNumTropas()-resAtaque.Length);

                    // Atacante ha conquistado
                    jugadores[jugAtq].haConquistado = true;

                    // Comprueba si ha eliminado al jugador defensor
                    if (!territorios.Exists(aux => aux.getPropietario() == jugDef)){
                        StartCoroutine(ShowError("¡Jugador " + jugadores[jugDef].userName + " eliminado!", 5));

                        jugadoresEliminados.Add(jugDef);
                        nJugadoresEliminados++;
                    }
                    break;
                }
            }
            else{
                // Gana defensor
                Debug.Log("Dado " + i + ": Gana defensor!");
                tAtq.setNumTropas(tAtq.getNumTropas()-1);
            }
        }
    }
}


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
    public Territorio attackingFrom {get; set;} // Solo tiene valor en la fase de ataque del turno
    public Territorio movingFrom {get; set;} // Solo tiene valor en la fase de ataque del turno

    private Jugada ultJugada;

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
        
        territorios = getListaTerritorios();

        nJugadoresEliminados = 0;
        jugadoresEliminados = new List<int>();

        attackingFrom = null;
        movingFrom = null;
        ultJugada = null;
    }

    void Update(){
        while(jugadas.hayJugadas()){
            procesarJugada(jugadas.ultimaJugada());
        }
    }

    List<Territorio> getListaTerritorios(){
        List<Transform> continentes = new List<Transform>();
        continentes.Add(transform.Find("Mapa").Find("Africa"));
        continentes.Add(transform.Find("Mapa").Find("Asia"));
        continentes.Add(transform.Find("Mapa").Find("Europa"));
        continentes.Add(transform.Find("Mapa").Find("Oceania"));
        continentes.Add(transform.Find("Mapa").Find("Sudamerica"));
        continentes.Add(transform.Find("Mapa").Find("Norteamerica"));
        
        List<Territorio> res = new List<Territorio>();
        foreach( Transform continente in continentes){
            foreach (Transform territorio in continente){
                res.Add(territorio.gameObject.GetComponent<Territorio>());
            }
        }
        return res;
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
            Jugador pl =  jugadores[j.userId];
            if (todosTerritoriosConquistados() && t.getPropietario()!=j.userId){
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


    private void utilizarCartas( JugadaUtilizarCartas j){
        if(turno.checkTurno(j)){

        }
    }

    private void ataqueSincrono(JugadaAtaqueSincrono j){
        if (turno.checkTurno(j)){
            Territorio atacante =  territorios.Find(aux => aux.id == j.territorioAtacante);
            Territorio atacado =  territorios.Find(aux => aux.id == j.territorioAtacado);

            if(atacante.getPropietario() != j.userId){
                Debug.Log("Error en ataqueSincrono: El jugador no es propietario del territorio atacante.");
                return;
            }
            if(atacado.getPropietario() == j.userId){
                Debug.Log("Error en ataqueSincrono: El jugador no puede atacar su propio territorio.");
                return;
            }
            if(!atacante.TerrColindantes.Contains(atacado)){
                Debug.Log("Error en ataqueSincrono: Los territorios deben ser colindantes.");
                return;
            }

            if(atacado.getPropietario() == myId){
                // TODO: Mostrar popup defensa
                StartCoroutine(ShowError("Estas siendo atacado, ¡Defiendete!", 5));
            }
        }
    }

    private void defensaSincrona(JugadaDefensaSincrona j){
        if(turno.checkTurno(j)){
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
            if(atacado.getPropietario() != j.userId){
                Debug.Log("Error en defensaSincriona: El jugador no es propietario del territorio atacado.");
                return;
            }

            decidirBatalla(ataque.resultadoDadosAtaque, j.resultadoDadosDefensa, ataque.territorioAtacante, ataque.territorioAtacado);
        }
    }

    private void ataqueAsincrono(JugadaAtaqueAsincrono j){
        if(turno.checkTurno(j)){
            Territorio atacante =  territorios.Find(aux => aux.id == j.territorioAtacante);
            Territorio atacado =  territorios.Find(aux => aux.id == j.territorioAtacado);

            if(atacante.getPropietario() != j.userId){
                Debug.Log("Error en ataqueAsincrono: El jugador no es propietario del territorio atacante.");
                return;
            }
            if(atacado.getPropietario() == j.userId){
                Debug.Log("Error en ataqueAsincrono: El jugador no puede atacar su propio territorio.");
                return;
            }
            if(!atacante.TerrColindantes.Contains(atacado)){
                Debug.Log("Error en ataqueAsincrono: Los territorios deben ser colindantes.");
                return;
            }

            decidirBatalla(j.resultadoDadosAtaque, j.resultadoDadosDefensa, j.territorioAtacante, j.territorioAtacado);
        }
    }
    
    private void pedirCarta(JugadaPedirCarta j){
        if(turno.checkTurno(j)){
            jugadores[j.userId].cartas.addCarta(j.cartaRecibida); 
        }
    }

    private void finPartida(JugadaFinPartida j){

    }


    // Funciones auxiliares
    private void decidirBatalla(int[] resAtaque, int[] resDefensa, string idTerrAtaque, string idTerrDefensa){
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
                tDef.setNumTropas(tDef.getNumTropas()-1);
                
                if(tDef.intentarConquistar(jugAtq)){
                    // Consigue conquistar
                    tDef.setNumTropas(resAtaque.Length);
                    jugadores[jugAtq].haConquistado = true;

                    // Comprueba si ha eliminado al jugador defensor
                    if (!territorios.Exists(aux => aux.getPropietario() == jugDef)){
                        jugadoresEliminados.Add(jugDef);
                        nJugadoresEliminados++;
                    }
                }
            }
            else{
                // Gana defensor
                tAtq.setNumTropas(tAtq.getNumTropas()-1);
            }
        }
    }

}


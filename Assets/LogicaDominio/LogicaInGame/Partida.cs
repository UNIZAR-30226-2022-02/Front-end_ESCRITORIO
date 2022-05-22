using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicaInGame.Jugadas;

public class Partida : MonoBehaviour
{
    List<Jugador> jugadores;
    List<Territorio> territorios;

    int idPartida;
    bool partidaSincrona;
    bool faseInicial;
    int nVecesCartasUsadas;
    int turnoActual;

    // Jugadores eliminados, gestion partida
    int nJugadoresEliminados;
    List<int> jugadoresEliminados;

    // Start is called before the first frame update
    void Start()
    {
        territorios = new List<Territorio>();
        faseInicial = true;
        nVecesCartasUsadas = 0;
        turnoActual = 0;
        nJugadoresEliminados = 0;
        jugadoresEliminados = new List<int>();
    }

    // ============================
    // - PROCESAMIENTO DE JUGADAS -
    // ============================
    private void procesarJugada(JugadaCrearPartida j){
        Debug.Log("Ejecutando jugada concreta");
        idPartida = j.idPartida; 
        jugadores = j.listaJugadores;
        partidaSincrona = j.partidaSincrona;
    }

    private void procesarJugada(Jugada j){
       
        if (j.GetType() == new JugadaCrearPartida().GetType()){
            crearPartida((JugadaCrearPartida) j);
        }
        else if (j.GetType() == new JugadaFinTurno().GetType()){
            finTurno((JugadaFinTurno) j);
        }
        else if (j.GetType() == new JugadaPonerTropas().GetType()){
            ponerTropas((JugadaPonerTropas) j);
        }
        else if (j.GetType() == new JugadaMoverTropas().GetType()){
            moverTropas((JugadaMoverTropas) j);
        }
        else if (j.GetType() == new JugadaUtilizarCartas().GetType()){
            utilizarCartas((JugadaUtilizarCartas) j);
        }
        else if (j.GetType() == new JugadaAtaqueSincrono().GetType()){
            ataqueSincrono((JugadaAtaqueSincrono) j);
        }
        else if (j.GetType() == new JugadaDefensaSincrona().GetType()){
            defensaSincrona((JugadaDefensaSincrona) j);
        }
        else if (j.GetType() == new JugadaAtaqueAsincrono().GetType()){
            ataqueAsincrono((JugadaAtaqueAsincrono) j);
        }
        else if (j.GetType() == new JugadaPedirCarta().GetType()){
               pedirCarta((JugadaPedirCarta) j);
        }
        else if (j.GetType() == new JugadaFinPartida().GetType()){
            finPartida((JugadaFinPartida) j);
        }
        else{
            Debug.Log("Error: " + j.GetType() + " no es un tipo de jugada valida.");
        }
    }

    private void crearPartida( JugadaCrearPartida j){

    }

    private void finTurno(JugadaFinTurno j){

    }

    private void ponerTropas(JugadaPonerTropas j){

    }

    private void moverTropas(JugadaMoverTropas j){

    }

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

    }



}


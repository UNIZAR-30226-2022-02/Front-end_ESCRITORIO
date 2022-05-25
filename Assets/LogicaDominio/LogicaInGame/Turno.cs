using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LogicaInGame.Jugadas;

public class Turno : MonoBehaviour
{
    private Partida myGame;
    private ColaJugadas colaJugadas;
    private WebSocketHandler wsHandler;

    bool faseInicial;
    int turnoActual;
    int faseTurno; // 0-Distribucion, 1-Ataque, 2-Fortificacion


    // GUI
    private GameObject distribucion, ataque, fortificacion;
    private Vector3 tamPeq, tamGrand;
    int ultFase;

    // ====================
    // - Metodos Publicos -
    // ====================
    public void avanzaTurno(){
        do{
            turnoActual++;
            turnoActual %= myGame.nJugadores;
        }
        while(myGame.jugadoresEliminados.Contains(turnoActual));

        if (!faseInicial){
            myGame.jugadores[turnoActual].anadirTropasTurno();
        }
        else{
            // Fase inicial 
            int totalTropas = 0;
            foreach (Jugador j in myGame.jugadores){
                totalTropas += j.getNTropasSinColocar();
            }
            
            if(totalTropas == 0){
                // No quedan tropas por colocar
                Debug.Log("Turno: Fin de la fase inicial.");
                faseInicial =false;
                myGame.jugadores[turnoActual].anadirTropasTurno();
            }
            
        }
    }

    public bool checkTurno(Jugada j){
        if (j.userId == turnoActual){
            return true;
        }
        else{
            Debug.Log("Error jugada " + j + ": El usuario que realiza la jugada no es el que tiene el turno");
            return false;
        }
    }

    public bool getFaseInicial(){
        return faseInicial;
    }
    public int getTurnoActual(){
        return turnoActual;
    }
    public int getFaseTurno(){
        return faseTurno;
    }

    // ===========================================================================
    
    void Start()
    {
        myGame = this.transform.parent.gameObject.GetComponent<Partida>();
        colaJugadas = this.transform.parent.parent.gameObject.GetComponent<ColaJugadas>();
        wsHandler = this.transform.parent.parent.gameObject.GetComponent<WebSocketHandler>();

        faseInicial = true;
        turnoActual = 0;
        faseTurno = 0;

        // GUI
        distribucion = this.transform.Find("InfoTurno").Find("Distribucion").Find("bloque").gameObject;
        ataque = this.transform.Find("InfoTurno").Find("Ataque").Find("bloque").gameObject;
        fortificacion = this.transform.Find("InfoTurno").Find("Fortificacion").Find("bloque").gameObject;

        tamPeq = transform.localScale;
        tamGrand = tamPeq + new Vector3(+8.0f,+8.0f,+0.0f);

        ultFase = -1;
    }

    void Update(){
        if(faseTurno != ultFase){
            switch(faseTurno){
                case 0:
                    distribucion.SetActive(true);
                    ataque.SetActive(false);
                    fortificacion.SetActive(false);
                    break;
                case 1:
                    distribucion.SetActive(true);
                    ataque.SetActive(true);
                    fortificacion.SetActive(false);
                    break;
                case 2:
                    distribucion.SetActive(true);
                    ataque.SetActive(true);
                    fortificacion.SetActive(false);
                    break;
            }
            ultFase = faseTurno;
        }
    }

    // =========================
    // - Generacion de Jugadas -
    // =========================

    void OnMouseEnter(){
        if(turnoActual == myGame.myId){
            transform.localScale = tamGrand;
        }
    }

    void OnMouseExit(){
        transform.localScale = tamPeq;        
    }

    void OnMouseClick(){
        if(turnoActual == myGame.myId){
            avanzaFaseTurno();
        }

    }

    // Funcion que pasa a la siguiente fase del ataque
    // Desencadena el envio de una JugadaFinTurno si es necesario
    private void avanzaFaseTurno(){
        if (faseTurno == 0 && myGame.jugadores[turnoActual].getNTropasSinColocar()!=0 ){
            StartCoroutine(myGame.ShowError("¡Coloca todas tus tropas antes de atacar!", 3));
            return;
        }

        faseTurno ++;
        faseTurno %= 3;

        // Finaliza mi turno
        if (faseTurno == 0){
            Jugada j = new JugadaFinTurno(myGame.myId, myGame.idPartida); 
            wsHandler.notificaJugada(j);        
        }
    }

}

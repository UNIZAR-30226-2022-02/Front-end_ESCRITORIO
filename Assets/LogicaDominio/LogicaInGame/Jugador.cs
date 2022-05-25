using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogicaInGame.Cartas;

public class Jugador : MonoBehaviour
{
    private Partida myGame;
    private Turno turno;

    
    public int id;
    public Color32 myColor;
    public string userName { get; set;}
    public Cartas cartas { get; set; }
    private int nTropasSinColocar;
    
    public bool haConquistado { get; set;} // REINICIAR A FALSE EN CADA TURNO!

    // Interface
    private Text usernameText, nTropasText;
    private Image infoJugador;
    private int ultTurno;

    // ====================
    // - Metodos Publicos -
    // ====================

    public void anadirTropasTurno(){
        //TODO: anadir las tropas en funcion de los territorios
    }
    public void setNTropasSinColocar(int nTropas){
        nTropasSinColocar = nTropas;
        nTropasText.text = nTropas.ToString();
    }
    public int getNTropasSinColocar(){
        return nTropasSinColocar;
    }

    // =========================================================================
    void Start()
    {
        myGame = this.transform.parent.parent.gameObject.GetComponent<Partida>();
        turno = this.transform.parent.parent.Find("Turno").gameObject.GetComponent<Turno>();

        // Obtiene Texts
        usernameText = (Text) this.transform.Find("InfoJugador").Find("username").gameObject.GetComponent("Text");
        nTropasText = (Text) this.transform.Find("InfoJugador").Find("numTropas").gameObject.GetComponent("Text");
        infoJugador = (Image) this.transform.Find("InfoJugador").gameObject.GetComponent("Image");
        
        // inicializa algunos atributos
        cartas = new Cartas();
        haConquistado = false;

        // GUI
        this.GetComponent<Image>().color = myColor;
        ultTurno = -1;
    }


    void Update(){
        // Cambios de turno
        int turnoActual = turno.getTurnoActual();
        if( turnoActual!= ultTurno){
            // Colores (GUI)
            setColor(turnoActual);
            ultTurno = turnoActual;

            // Reset haConquistado
            if(turnoActual == id){
                haConquistado = false;
            }
        }
    }

    public void inicializa(string userName, int nJugadores){
        // Inicializa resto de atributos
        this.userName = userName;
        usernameText.text = userName;

        switch (nJugadores){
            case 3:
                setNTropasSinColocar(35);
                break;
            case 4:
                setNTropasSinColocar(30);
                break;
            case 5:
                setNTropasSinColocar(25);
                break;
            case 6:
                setNTropasSinColocar(20);
                break;
            default:
                setNTropasSinColocar(-1);
                Debug.Log("Error inicializando jugador " + id 
                + ": El numero de jugadores no puede ser" + nJugadores );
                break;
        }
        
    }

    // GUI

    private void setColor(int turnoActual){
        if(myGame.jugadoresEliminados.Contains(turnoActual)){
            infoJugador.color = new Color32(195,80,80,125);
            return;
        }

        if(turnoActual == this.id){
            infoJugador.color = new Color32(109,204,99,255);
        }
        else{
            infoJugador.color = new Color32(192,192,192,255);
        }
    }
}

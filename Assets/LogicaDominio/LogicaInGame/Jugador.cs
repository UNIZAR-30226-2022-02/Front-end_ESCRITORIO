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
        // Num territorios
        List<Territorio> misTerr = myGame.territorios.FindAll( aux => aux.getPropietario() == this.id);
        int nTerritorios = misTerr.Count;


        // Continentes
        int valContinentes = getTropasPorContinente(misTerr);

        int numTropas = Mathf.Max((nTerritorios/3) + valContinentes, 3);
        setNTropasSinColocar(numTropas);
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
        infoJugador.color = myColor;
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

    private int getTropasPorContinente(List<Territorio> misTerr){
        int total = 0;
        List<Territorio> asia = myGame.territorios.FindAll( aux => aux.continente == "asia");
        List<Territorio> europa = myGame.territorios.FindAll( aux => aux.continente == "europa");
        List<Territorio> africa = myGame.territorios.FindAll( aux => aux.continente == "africa");
        List<Territorio> oceania = myGame.territorios.FindAll( aux => aux.continente == "oceania");
        List<Territorio> sudAmerica = myGame.territorios.FindAll( aux => aux.continente == "sudamerica");
        List<Territorio> norteAmerica = myGame.territorios.FindAll( aux => aux.continente == "norteamerica");

        if(asia.TrueForAll( aux => misTerr.Contains(aux))){
            total += 7;
        }
        if(europa.TrueForAll( aux => misTerr.Contains(aux))){
            total += 5;
        }
        if(africa.TrueForAll( aux => misTerr.Contains(aux))){
            total += 3;
        }
        if(oceania.TrueForAll( aux => misTerr.Contains(aux))){
            total += 2;
        }
        if(sudAmerica.TrueForAll( aux => misTerr.Contains(aux))){
            total += 2;
        }
        if(norteAmerica.TrueForAll( aux => misTerr.Contains(aux))){
            total += 5;
        }

        return total;
    }

    // GUI

    private void setColor(int turnoActual){
        if(myGame.jugadoresEliminados.Contains(this.id)){
            this.GetComponent<Image>().color = new Color32(128,0,3,255);
            return;
        }

        if(turnoActual == this.id){
            this.GetComponent<Image>().color = new Color32(32,192,0,255);
        }
        else{
            this.GetComponent<Image>().color = new Color32(192,192,192,255);
        }
    }
}

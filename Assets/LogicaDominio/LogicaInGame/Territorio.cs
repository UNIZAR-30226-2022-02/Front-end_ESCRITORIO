using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogicaInGame.Jugadas;

[RequireComponent(typeof(PolygonCollider2D))] //Para que coja el contorno de cada pais para el cambio de color
public class Territorio : MonoBehaviour
{
    // Info pais
    public string id;
    public List<Territorio> TerrColindantes;
    
    // Info propietario
    int propietario;
    int numTropas;

    // Comunicacion externa
    private Partida myGame;
    private ColaJugadas colaJugadas;
    private WebSocketHandler wsHandler;
    private Turno turno;

    // GUI
    private Text numTropasText;
    private GameObject popUpElegirNumTropas;
    private Vector3 tamPeq, tamGrand;
    private SpriteRenderer sprite;

    // ====================
    // - Metodos Publicos -
    // ====================
    
    public bool intentarConquistar(int idConquistador){
        if(numTropas <= 0){
            propietario = idConquistador;
            return true;
        }

        return false;
    }

    public int getNumTropas(){
        return numTropas;
    }
    public void setNumTropas(int nuevoNumTropas){
        numTropas = nuevoNumTropas;
        numTropasText.text = numTropas.ToString();
    }
    public int getPropietario(){
        return propietario;
    }
    public void setPropietario( int nuevoPropietario){
        propietario = nuevoPropietario;
        Color newColor;
        switch(propietario){ // TODO: Poner colores
            case 0:
                newColor = new Color32();
                break;
            case 1:
                newColor = new Color32();
                break;
            case 2:
                newColor = new Color32();
                break;
            case 3:
                newColor = new Color32();
                break;
            case 4:
                newColor = new Color32();
                break;
            case 5:
                newColor = new Color32();
                break;
            default:
                newColor = new Color32();
                break;
        }
        sprite.color = newColor;
    }

    // ===========================================================================
    void Start(){
        myGame = this.transform.parent.parent.parent.gameObject.GetComponent<Partida>();
        colaJugadas = myGame.gameObject.transform.parent.GetComponent<ColaJugadas>();
        wsHandler = myGame.gameObject.transform.parent.GetComponent<WebSocketHandler>();
        turno = myGame.gameObject.transform.Find("Turno").gameObject.GetComponent<Turno>();

        propietario = -1;
        numTropas = 0;
        
        tamPeq = transform.localScale;
        tamGrand = tamPeq + new Vector3(+8.0f,+8.0f,+0.0f);

        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        numTropasText = this.transform.Find("tropas").Find("num").gameObject.GetComponent<Text>();
        popUpElegirNumTropas = this.transform.parent.parent.parent.Find("ElegirNumTropas").gameObject;

    }


    void Update(){
        if(myGame.movingFrom != null && myGame.movingFrom == this){
            transform.localScale = tamGrand;
        }
    }

    // =========================
    // - Generacion de Jugadas -
    // =========================

    // Acciones
    Acciones accionActual; // variable temporal
    enum Acciones{
        ponerEnVacio1,
        ponerEnMio1,
        ponerEnMioN,
        setTerritorioAtacante,
        setTerritorioAtacado,
        setOrigenMover,
        unSetOrigenMover,
        setDestinoMover,
        nula
    }

    // Verifica que el estado de la partida permita realizar alguna
    // jugada haciendo click en este territorio.
    // Si es posible realizar una jugada almacena el tipo en accionActual
    // para que OnClick() la realize sin necesidad de volver a verificar todo.
    void OnMouseEnter(){ 
        if(turno.getTurnoActual() == myGame.myId ){
            return;
        }

        // Fase inicial
        if(turno.getFaseInicial()){
            bool terrsConq = myGame.todosTerritoriosConquistados();
            int nTropasSinColocar = myGame.jugadores[myGame.myId].getNTropasSinColocar();
            // Colocar en terr vacio
            if(!terrsConq && propietario==-1 && nTropasSinColocar>0){
                transform.localScale = tamGrand;  
                accionActual = Acciones.ponerEnVacio1;
                return;   
            }
            // Colocar en mi territorio
            if(terrsConq && propietario==myGame.myId && nTropasSinColocar>0){
                transform.localScale = tamGrand;   
                accionActual = Acciones.ponerEnMio1;  
                return;   
            }
        }
        // Fase Ataque
        else{
            int ft = turno.getFaseTurno();
            int nTropasSinColocar = myGame.jugadores[myGame.myId].getNTropasSinColocar();
            // Distribucion
            if (ft==0 && propietario==myGame.myId){
                transform.localScale = tamGrand; 
                accionActual = Acciones.ponerEnMioN;  
                return;       
            }
            // Ataque
            if (ft==1 && propietario==myGame.myId){
                transform.localScale = tamGrand; 
                accionActual = Acciones.setTerritorioAtacante;  
                return;       
            }
            if(ft==1 && myGame.attackingFrom!=null){
                transform.localScale = tamGrand; 
                accionActual = Acciones.setTerritorioAtacado;  
                return;  
            }
            // Fortificacion
            if (ft==2 && propietario==myGame.myId){
                if(myGame.movingFrom==null && numTropas>1){
                    transform.localScale = tamGrand; 
                    accionActual = Acciones.setOrigenMover;
                    return;
                }
                else if(myGame.movingFrom==this){
                    transform.localScale = tamGrand; 
                    accionActual = Acciones.unSetOrigenMover;
                    return;
                }
                else{
                    transform.localScale = tamGrand; 
                    accionActual = Acciones.setDestinoMover;
                    return;
                }
            }
        }
    }

    void OnMouseExit(){
        transform.localScale = tamPeq;      
        accionActual = Acciones.nula;  
    }

    // Funcion que genera jugadas, las añade a la cola
    // y las comunica al servidor. Se gestiona:
    //      finTurno (SOLO de la FASE INICIAL)
    //      ponerTropas
    //      moverTropas
    //      ataqueSincrono
    //      ataqueAsincrono

    void OnMouseClick(){
        if(turno.getTurnoActual() != propietario){
            return;        
        }
        
        switch (accionActual){
            // -Fase inicial- 
            // --------------
            // Quedan territorios vacios
            case Acciones.ponerEnVacio1:
                Jugada j1 = new JugadaPonerTropas(myGame.myId, myGame.idPartida, this.id, 1);
                Jugada j2 = new JugadaFinTurno(myGame.myId, myGame.idPartida);
                wsHandler.notificaJugada(j1);
                wsHandler.notificaJugada(j2);
                break;

            // NO quedan territorios vacios
            case Acciones.ponerEnMio1:
                j1 = new JugadaPonerTropas(myGame.myId, myGame.idPartida, this.id, 1);
                j2 = new JugadaFinTurno(myGame.myId, myGame.idPartida);
                wsHandler.notificaJugada(j1);
                wsHandler.notificaJugada(j2);
                break;

            // -Fase Normal-
            // -------------
            
            // Distribucion
            case Acciones.ponerEnMioN:
                // obtener (y validar) numTropas
                
                break;

            // Ataque
            case Acciones.setTerritorioAtacante:
                break;
            case Acciones.setTerritorioAtacado:
                // obtener (y validar) numTropas + reset secuencia ataque
                break;

            // Fortificacion
            case Acciones.setOrigenMover:
                break;
            case Acciones.unSetOrigenMover:
                break;
            case Acciones.setDestinoMover:
                // obtener (y validar) numTropas + reset secuencia mover
                break;
        }

        // Resetea accion
        accionActual = Acciones.nula;
    }

}

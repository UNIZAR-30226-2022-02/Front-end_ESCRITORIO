using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace LogicaInGame.Jugadas
{
    // ===========
    // - Jugadas -
    // ===========
    [System.Serializable]
    public class Jugada 
    {
        public string type;
        public int userId;
        public int idPartida;

        public Jugada(string type, int userId, int idPartida){
            this.type = type;
            this.userId = userId;
            this.idPartida = idPartida;
        }

        public virtual string ToString(){
            return type + ": userId = " + userId + " - idPartida = " + idPartida;
        }
            
    // Recibe un string que representa el json de la jugada 
    // y devuelve una Jugada con la instancia del tipo heredado
    // correspondiente.
    // Devuelve null si el tipo de jugada no es correcto
    public static Jugada parseJsonJugada(string json){
        Jugada j =  JsonUtility.FromJson<Jugada>(json);
        Jugada result;
        switch (j.type){
            case "crearPartida":
                result = JsonUtility.FromJson<JugadaCrearPartida>(json);
                break;
            default:
                Debug.Log("Error parseJsonJugada: " + j.type + " no es un tipo valido.");
                return null;
        }

        //Debug.Log("Ejecutando parseJson, devolviendo " + result.ToString());
        return result;
    }   
    }

    [System.Serializable]
    public class JugadaCrearPartida : Jugada
    {
        public string[] listaJugadores;
        public bool partidaSincrona;
        
        public JugadaCrearPartida(int userId, int idPartida, string[] listaJugadores, bool partidaSincrona)
        : base("crearPartida", userId, idPartida)
        {
            this.listaJugadores = listaJugadores;
            this.partidaSincrona = partidaSincrona;
        }

        public override string ToString(){
            string lista = "[ ";
            for( int i = 0; i<listaJugadores.Length; i++){
                lista += listaJugadores[i] + ",";
            }
            lista += "]";
            return base.ToString() + " - listaJugadores = " + lista + " - partidaSincrona = " + partidaSincrona; 
        }
    }

    [System.Serializable]
    public class JugadaFinTurno : Jugada
    {
        public JugadaFinTurno(int userId, int idPartida)
        : base("finTurno", userId, idPartida)
        {}
    }

    public class JugadaPonerTropas : Jugada
    {
        public string idTerritorio; // TODO: decidir tipo de dato
        public int numTropas;

        public JugadaPonerTropas(int userId, int idPartida, string idTerritorio, int numTropas)
        : base("ponerTropas", userId, idPartida)
        {
            this.idTerritorio = idTerritorio;
            this.numTropas = numTropas;
        }

        public override string ToString(){
            return base.ToString() + " - idTerritorio = " + idTerritorio + " - numTropas = " + numTropas; 
        }
    }

    public class JugadaMoverTropas : Jugada
    {
        public string idTerritorioOrigen { get; set; }
        public string idTerritorioDestino { get; set; }
        public int numTropas { get; set; } 

        public JugadaMoverTropas(int userId, int idPartida, string idTerritorioOrigen, string idTerritorioDestino, int numTropas)
        : base("moverTropas", userId, idPartida)
        {
            this.idTerritorioOrigen = idTerritorioOrigen;
            this.idTerritorioDestino = idTerritorioDestino;
            this.numTropas = numTropas;
        }

        public override string ToString(){
            return base.ToString() + " - idTerritorioOrigen = " + idTerritorioOrigen 
            + " - idTerritorioDestino = " + idTerritorioDestino + " - numTropas = " + numTropas; 
        }
    }

    /*
    public class JugadaUtilizarCartas : Jugada
    {

    }

    public class JugadaAtaqueSincrono : Jugada
    {

    }

    public class JugadaDefensaSincrona : Jugada
    {

    }

    public class JugadaAtaqueAsincrono : Jugada
    {

    }

    public class JugadaPedirCarta : Jugada
    {

    }

    public class JugadaFinPartida : Jugada
    {

    }
    */
}
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
        public string userId;
        public int idPartida;

        public Jugada(string type, string userId, int idPartida){
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
                case "finTurno":
                    result = JsonUtility.FromJson<JugadaFinTurno>(json);
                    break;
                case "ponerTropas":
                    result = JsonUtility.FromJson<JugadaPonerTropas>(json);
                    break;
                case "moverTropas":
                    result = JsonUtility.FromJson<JugadaMoverTropas>(json);
                    break;
                case "utilizarCartas":
                    result = JsonUtility.FromJson<JugadaUtilizarCartas>(json);
                    break;
                case "ataqueSincrono":
                    result = JsonUtility.FromJson<JugadaAtaqueSincrono>(json);
                    break;
                case "defensaSincrona":
                    result = JsonUtility.FromJson<JugadaDefensaSincrona>(json);
                    break;
                case "ataqueAsincrono":
                    result = JsonUtility.FromJson<JugadaAtaqueAsincrono>(json);
                    break;
                case "pedirCarta":
                    result = JsonUtility.FromJson<JugadaPedirCarta>(json);
                    break;
                case "finPartida":
                    result = JsonUtility.FromJson<JugadaFinPartida>(json);
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
        
        public JugadaCrearPartida(string userId, int idPartida, string[] listaJugadores, bool partidaSincrona)
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
        public JugadaFinTurno(string userId, int idPartida)
        : base("finTurno", userId, idPartida)
        {}
    }

    public class JugadaPonerTropas : Jugada
    {
        public string idTerritorio; // TODO: decidir tipo de dato
        public int numTropas;

        public JugadaPonerTropas(string userId, int idPartida, string idTerritorio, int numTropas)
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
        public string idTerritorioOrigen;
        public string idTerritorioDestino;
        public int numTropas; 

        public JugadaMoverTropas(string userId, int idPartida, string idTerritorioOrigen, string idTerritorioDestino, int numTropas)
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

    
    
    public class JugadaUtilizarCartas : Jugada
    {
        public int[] cartasUtilizadas;

        public JugadaUtilizarCartas(string userId, int idPartida, int[] cartasUtilizadas)
        : base("utilizarCartas", userId, idPartida)
        {
            this.cartasUtilizadas = cartasUtilizadas;
        }

        public override string ToString(){
            return base.ToString() + " - cartasUtilizadas = " + cartasUtilizadas; 
        }
    }

    public class JugadaAtaqueSincrono : Jugada
    {
        public string territorioAtacante;
        public string territorioAtacado;
        public int[] resultadoDadosAtaque;

        public JugadaAtaqueSincrono(string userId, int idPartida, string territorioAtacante, string territorioAtacado, int[] resultadoDadosAtaque)
        : base("ataqueSincrono", userId, idPartida)
        {
            this.territorioAtacante = territorioAtacante;
            this.territorioAtacado = territorioAtacado;
            this.resultadoDadosAtaque = resultadoDadosAtaque;
        }

        public override string ToString(){
            return base.ToString() + " - territorioAtacante = " + territorioAtacante 
            + " - territorioAtacado = " + territorioAtacado + " - resultadoDadosAtaque = " + resultadoDadosAtaque; 
        }
    }

    public class JugadaDefensaSincrona : Jugada
    {
        public string territorioAtacante;
        public string territorioAtacado;
        public int[] resultadoDadosDefensa;

        public JugadaDefensaSincrona(string userId, int idPartida, string territorioAtacante, string territorioAtacado, int[] resultadoDadosDefensa)
        : base("defensaSincrona", userId, idPartida)
        {
            this.territorioAtacante = territorioAtacante;
            this.territorioAtacado = territorioAtacado;
            this.resultadoDadosDefensa = resultadoDadosDefensa;
        }

        public override string ToString(){
            return base.ToString() + " - territorioAtacante = " + territorioAtacante 
            + " - territorioAtacado = " + territorioAtacado + " - resultadoDadosDefensa = " + resultadoDadosDefensa; 
        }
    }

    public class JugadaAtaqueAsincrono : Jugada
    {
        public string territorioAtacante;
        public string territorioAtacado;
        public int[] resultadoDadosAtaque;
        public int[] resultadoDadosDefensa;

        public JugadaAtaqueAsincrono(string userId, int idPartida, string territorioAtacante, string territorioAtacado,
                                     int[] resultadoDadosAtaque, int[] resultadoDadosDefensa)
        : base("ataqueAsincrono", userId, idPartida)
        {
            this.territorioAtacante = territorioAtacante;
            this.territorioAtacado = territorioAtacado;
            this.resultadoDadosAtaque = resultadoDadosAtaque;
            this.resultadoDadosDefensa = resultadoDadosDefensa;
        }

        public override string ToString(){
            return base.ToString() + " - territorioAtacante = " + territorioAtacante 
            + " - territorioAtacado = " + territorioAtacado + " - resultadoDadosAtaque = " 
            + resultadoDadosAtaque + "resultadoDadosDefensa = " +  resultadoDadosDefensa;
        }
    }

    public class JugadaPedirCarta : Jugada
    {
        public int cartaRecibida;

        public JugadaPedirCarta(string userId, int idPartida, int cartaRecibida)
        : base ("pedirCarta", userId, idPartida)
        {
            this.cartaRecibida = cartaRecibida;
        }
        public override string ToString(){
            return base.ToString() + " - cartaRecibida = " + cartaRecibida;
        }
        
    }

    public class JugadaFinPartida : Jugada
    {
        public List<string> listaJugadores;
        public JugadaFinPartida(string userId, int idPartida, List<string> listaJugadores)
        : base ("finPartida", userId, idPartida)
        {
            this.listaJugadores = listaJugadores;
        }
        public override string ToString(){
            return base.ToString() + " - listaJugadores = " + listaJugadores;
        }
    }
    
}
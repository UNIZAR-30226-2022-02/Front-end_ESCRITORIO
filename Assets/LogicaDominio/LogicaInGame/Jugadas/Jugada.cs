using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicaInGame.Jugadas
{
    public class Jugada 
    {
        public string type;
        public int userId { get; set; }
        public int idPartida { get; set; }

        public Jugada(string type, int userId, int idPartida){
            this.type = type;
            this.userId = userId;
            this.idPartida = idPartida;
        }
        
    }

    public class JugadaCrearPartida : Jugada
    {
        public List<string> listaJugadores  { get; set; }
        public bool partidaSincrona { get; set; }
        
        public JugadaCrearPartida(int userId, int idPartida, List<string> listaJugadores, bool partidaSincrona){
            super("CrearPartida", userId, idPartida);
            this.listaJugadores = listaJugadores;
            this.partidaSincrona = partidaSincrona;
        }
    }

    public class JugadaFinTurno : Jugada
    { }

    public class JugadaPonerTropas : Jugada
    {
        public string idTerritorio { get; set; } // TODO: decidir tipo de dato
        public int numTropas { get; set; } 
    }

    public class JugadaMoverTropas : Jugada
    {
        public string idTerritorioOrigen { get; set; } // TODO: decidir tipo de dato
        public string idTerritorioDestino { get; set; } // TODO: decidir tipo de dato
        public int numTropas { get; set; } 
    }

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
}
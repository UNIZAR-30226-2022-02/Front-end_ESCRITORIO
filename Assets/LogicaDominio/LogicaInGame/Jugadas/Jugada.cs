using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicaInGame.Jugadas
{
    public class Jugada 
    {
        public int userId { get; set; }
        public int idPartida { get; set; }
        
    }

    public class JugadaCrearPartida : Jugada
    {
        public List<string> listaJugadores  { get; set; }
        public int idPrimerJugador { get; set; }
        public bool partidaSincrona { get; set; }
        
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
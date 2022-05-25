using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LogicaInGame.Jugadas;

public class ColaJugadas : MonoBehaviour
{
    private Queue<Jugada> myQueue;
    
    // ====================
    // - Metodos publicos -
    // ====================

    public void nuevaJugada(Jugada j){
        myQueue.Enqueue(j);
    }

    public Jugada ultimaJugada(){
        return myQueue.Dequeue();
    }

    public bool hayJugadas(){
        return myQueue.Count != 0;
    }

    void Start()
    {
        myQueue = new Queue<Jugada>();

        // Prueba
        
        

    }

    private void pruebaFaseInicial(){
        string[] paises = new string[]{
            "japan", "india", "yakursk", "kamchatka", "siberia",
            "ural", "afghanistan", "middle_east", "siam", "china",
            "mongolia", "irkutsk", "north_africa", "madagascar", 
            "east_africa", "congo", "south_africa", "egypt", 
            "eastern_australia", "western_australia", "indonesia", 
            "new_guinea", "alaska", "alberta", "northwest_territory", 
            "ontario", "eastern_united_states", "western_united_states", 
            "central_america", "quebec", "greenland", "venezuela", "brazil",
            "argentina", "peru", "great_britain", "iceland", "scandinavia",
            "ukraine", "southern_europe", "western_europe", "northern_europe"
        };

        int i=0;
        foreach(string pais in paises){
            nuevaJugada(new JugadaPonerTropas());
        }
    }
    
}

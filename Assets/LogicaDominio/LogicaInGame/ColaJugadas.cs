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
        StartCoroutine(prueba());
        

    }
    private IEnumerator prueba(){
        yield return pruebaFaseInicial();
        yield return pruebaAtaque();
    }
    private IEnumerator pruebaFaseInicial(){
        string[] paises = new string[]{
            "iceland", "japan", "india", "yakursk", "kamchatka", "siberia",
            "ural", "afghanistan", "middle_east", "siam", "china",
            "mongolia", "irkutsk", "north_africa", "madagascar", 
            "east_africa", "congo", "south_africa", "egypt", 
            "eastern_australia", "western_australia", "indonesia", 
            "new_guinea", "alaska", "alberta", "northwest_territory", 
            "ontario", "eastern_united_states", "western_united_states", 
            "central_america", "quebec", "greenland", "venezuela", "brazil",
            "argentina", "peru", "great_britain", "scandinavia",
            "ukraine", "southern_europe", "western_europe", "northern_europe"
        };

        myQueue.Enqueue(new JugadaCrearPartida(-1, 0, new string[]{"jesus", "juan", "sergio"}, true));

        int i=0;
        foreach(string pais in paises){
            myQueue.Enqueue(new JugadaPonerTropas(i, 0, pais, 1));
            myQueue.Enqueue(new JugadaFinTurno(i,0));
            i++;
            i %= 3;
            yield return new WaitForSeconds(0);
        }

        for(int j=0; j<3; j++){
            myQueue.Enqueue(new JugadaPonerTropas(j, 0, paises[j], 21));
            myQueue.Enqueue(new JugadaFinTurno(j,0));
        }
    }

    private IEnumerator pruebaAtaque(){
        //myQueue.Enqueue(new JugadaPonerTropas(0, 0, "iceland", 10));
        
        //myQueue.Enqueue(new JugadaAtaqueSincrono(0,0, "iceland", "scandinavia", new int[]{3,5,1}));
        //myQueue.Enqueue(new JugadaDefensaSincrona(1,0, "iceland", "scandinavia", new int[]{6,2,1}));

        yield return new WaitForSeconds(0);
    }
    
}

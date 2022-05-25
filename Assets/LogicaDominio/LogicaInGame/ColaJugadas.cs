using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LogicaInGame.Jugadas;

public class ColaJugadas : MonoBehaviour
{
    private Queue<Jugada> myQueue;

    void Start()
    {
        myQueue = new Queue<Jugada>();
        myQueue.Enqueue(new JugadaCrearPartida(-1, 0, new string[]{"jesus", "juan", "sergio"}, true));
        myQueue.Enqueue(new JugadaPonerTropas(0, 0, "af1", 1));

    }

    public void nuevaJugada(Jugada j){
        myQueue.Enqueue(j);
    }

    public Jugada ultimaJugada(){
        return myQueue.Dequeue();
    }

    public bool hayJugadas(){
        return myQueue.Count != 0;
    }

    
}

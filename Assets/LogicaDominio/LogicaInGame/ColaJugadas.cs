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

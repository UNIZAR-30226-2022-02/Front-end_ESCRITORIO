using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicaInGame.Cartas;

public class Jugador : MonoBehaviour
{
    int id;
    string userName;
    Cartas cartas;
    int nTropasSinColocar;
    
    bool haConquistado; // REINICIAR A FALSE EN CADA TURNO!


    // Start is called before the first frame update
    //Podriamos pasar desde el main un parametro con el nombre e inicializar el id con GameObjet.Find("parametro")
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

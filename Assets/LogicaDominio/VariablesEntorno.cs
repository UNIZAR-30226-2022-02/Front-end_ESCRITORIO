using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesEntorno : MonoBehaviour
{
    //Script que introduce las variables de entorno que se necesitaran
    public string myUsername {get; set;}
    public bool estoyEnPartida {get; set;}
    public bool mapaComprado {get; set;}
    public bool mapaSeleccionado {get; set;}
    public bool fichaComprada {get; set;}
    public bool fichaSeleccionada  {get; set;}
    public int idPartida {get; set;}
    
    // Start is called before the first frame update
    void Start()
    {
        myUsername = "user1";
        estoyEnPartida = false;
        mapaComprado = false;
        mapaSeleccionado = false;
        fichaComprada = false;
        fichaSeleccionada = false;
        idPartda = -1;
    }
}
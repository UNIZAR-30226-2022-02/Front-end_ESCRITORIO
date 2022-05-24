using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesEntorno : MonoBehaviour
{
    //Script que introduce las variables de entorno que se necesitaran
    public string myUsername; 
    public bool estoyEnPartida,mapaComprado,mapaSeleccionado,fichaComprada,fichaSeleccionada;
    
    public void setUsername(string username)
    {
        myUsername = username;
    }

    public void setEstoyEnPartida(bool enPartida)
    {
       estoyEnPartida = enPartida; 
    }

    public void setMapaComprado()
    {
        mapaComprado = true;
    }

    public void setmapaSeleccionado(bool seleccionMapa)
    {
        mapaSeleccionado = seleccionMapa;
    }

    public void setFichaComprada()
    {
        fichaComprada = true;
    }

    public void setFichaSeleccionada(bool seleccionFicha)
    {
        fichaSeleccionada = seleccionFicha;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        estoyEnPartida = false;
    }
}
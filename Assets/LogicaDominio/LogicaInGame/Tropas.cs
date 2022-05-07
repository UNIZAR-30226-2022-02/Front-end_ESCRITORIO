using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tropas
{
    //Ataca un jugador a otro. Se tiran los dados y 
    //si gana el atacante devuelve true, sino devuelve false
    /*bool atacar(int numDadosAtaque, int numDadosDefensor,){
        //Inicializados default (0)
        int numMayorAtaque, numMayorDefensa,DadoActual;
        
        //Tirar dados defensa
        for(int i = 0; i < numDadosDefensor; i++){
            DadoActual = rand(1,7);
            //mostrarNumeroDado();
            if(DadoActual > numMayorDefensa){
                numMayorDefensa = DadoActual;
            }
        }
        //Tirar dados ataque
        for(int i = 0; i < numDadosAtaque; i++){
            DadoActual = rand(1,7);
            //mostrarNumeroDado();
            if(DadoActual > numMayor){
                numMayorAtaque = DadoActual;
            }
        }

        if(numMayorAtaque >= numMayorDefensa){
            return true;
        }
        return false;

    }
    //Continentes[0] = Europa
    //Continentes[1] = Oceania
    //Continentes[2] = Asia
    //Continentes[3] = Africa
    //Continentes[4] = NorteAmerica
    //Continentes[5] = Sudamerica
    int CalcNuevasTropas(int numTerritorios,bool[] continentes){
        int nuevasTropas = numTerritorios/3;
        if(continentes[0] == true){
            nuevasTropas += 5;
        }
        if(continentes[1] == true){
            nuevasTropas += 2;
        }
        if(continentes[2] == true){
            nuevasTropas += 7;
        }
        if(continentes[3] == true){
            nuevasTropas += 3;   
        }
        if(continentes[4] == true){
            nuevasTropas += 5;
        }
        if(continentes[5] == true){
            nuevasTropas += 2;
        }
        return nuevasTropas;
    }*/
}

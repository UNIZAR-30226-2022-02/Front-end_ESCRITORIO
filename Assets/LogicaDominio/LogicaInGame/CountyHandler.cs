using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] //Para que coja el contorno de cada pais para el cambio de color
public class CountyHandler : MonoBehaviour
{
    //Tamaño (para el zoom)
    private Vector3 tamano;

    private string id,propietario;
    private int numTropas;
    private List<string> TerrColindantes;
    //void Start(){}
    //void Update(){}
    string getId(){
        return id;
    }

    string getPropietario(){
        return propietario;
    }

    int getNumTropas(){
        return numTropas;
    }

    List<string> getTerrColindantes(){
        return TerrColindantes;
    }

    void OnMouseEnter(){
        tamano = new Vector3(+10.0f,+10.0f,+0.0f);
        transform.localScale += tamano;
    }

    void OnMouseExit(){
        tamano = new Vector3(-10.0f,-10.0f,-0.0f);
        transform.localScale += tamano;
    }
}

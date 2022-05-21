using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] //Para que coja el contorno de cada pais para el cambio de color
public class Territorio : MonoBehaviour
{
    private string id,propietario;
    private int numTropas;
    private List<string> TerrColindantes;

    //Tamaño (para el zoom)
    private Vector3 tamano;

    private SpriteRenderer sprite;
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

    //void CambiarColor(){
    void OnMouseClick(){
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.red);
    }

    //Nos pasan el color como parámetro según el jugador que posea el territorio
    void CambiarColor(Color color){
        sprite = GetComponent<SpriteRenderer>();
        
        //Color color = new Color(0.2F, 0.3F, 0.4F);
        sprite.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] //Para que coja el contorno de cada pais
public class CountyHandler : MonoBehaviour
{

    private SpriteRenderer sprite;
    //[SerializabelField] ZoomCamera Cam;
    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start(){}
    void Update(){}
    void OnMouseEnter(){
        //Cam = FindObjectOfType<ZoomCamera>();
        //Cam.Acercar(Sprite);
    }

    void OnMouseExit(){
        //Camera.main.transform.position.x - 20;
        //Camera.main.transform.position.y - 20;
    }
}

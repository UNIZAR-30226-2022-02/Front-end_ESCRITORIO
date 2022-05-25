using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacionCarga : MonoBehaviour {
    public Texture2D[] frames;
    public int fps = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int index = (int)(Time.time * fps) % frames.Length;
        //GetComponent<Material>().mainTexture = frames[index];
        GetComponent<Renderer>().material.mainTexture = frames[index];
    }
}

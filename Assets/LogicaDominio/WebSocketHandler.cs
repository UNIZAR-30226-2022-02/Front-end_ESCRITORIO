using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SocketIO;
using LogicaInGame.Jugadas;

public class WebSocketHandler : MonoBehaviour
{
    private SocketIOComponent socket;
    private ColaJugadas colaJugadas;

    // ====================
    // - Metodos publicos -
    // ====================
    public void registrarme(string userName){
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["username"] = userName;
        socket.Emit("registro", new JSONObject(data));
        Debug.Log("WebSocket: Registro OK!");
    }

    public void notificaJugada(Jugada j){
        string json = JsonUtility.ToJson(j);
        socket.Emit("nueva_jugada", new JSONObject(json));
    }


    // Start is called before the first frame update
    void Start()
    {
        colaJugadas = this.GetComponent<ColaJugadas>();

        GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
        
		socket.On("nueva_jugada", nuevaJugada);  
        socket.On("clientes", nuevaJugada);  
    }


    private void nuevaJugada(SocketIOEvent e){
        Debug.Log("WebSocketHandler: Encolando nueva jugada...");
        string json = e.data.ToString();
        Jugada j = Jugada.parseJsonJugada(json);        
        colaJugadas.nuevaJugada(j);
    }

}

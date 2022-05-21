using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SocketIO;

public class WebSocketHandler : MonoBehaviour
{
    private SocketIOComponent socket;


    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
        Debug.Log("WebSocket component obtenido" + socket);

		socket.On("nueva_jugada", nuevaJugada);
		socket.On("clientes", nuevaJugada);
        
        register();
        Debug.Log("WebSocket: Registro OK!");
        
    }

    private void register(){
        socket.Emit("register"); 
    }

    private void nuevaJugada( SocketIOEvent e){
        Debug.Log("WebSocket: Pruebaaa");
        return;
    }
}

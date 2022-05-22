using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SocketIO;
using AppLogic.Jugadas;

public class WebSocketHandler : MonoBehaviour
{

    private SocketIOComponent socket;

    private Partida myGame;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
        Debug.Log("WebSocket component obtenido" + socket);

        socket.Connect();
        
		socket.On("nueva_jugada", nuevaJugada);
		socket.On("clientes", nuevaJugada);
        
    }

    public void registrarme(string userName){
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["username"] = userName;
        socket.Emit("registro", new JSONObject(data));
        Debug.Log("WebSocket: Registro OK!");
    }

    public void notificaJugada(Jugada j){

        socket.Emit(nueva_jugada, new JSONObject(j));
    }

    private void nuevaJugada( SocketIOEvent e){
        Jugada j = JsonUtility.fromJson<Jugada>(e.data());
        Debug.Log(j);
        myGame.procesarJugada(j);
    }

}

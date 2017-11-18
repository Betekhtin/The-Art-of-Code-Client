using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

public class SocketController : MonoBehaviour {

    private static SocketController instance = null;
    private static Socket socket;
    private static string server = "http://91.225.131.223:8080";

    void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public Socket getSocket() {
        return socket;
    }

    void Start () {
		socket = IO.Socket(server);
        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected to " + server);
        });
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    public GameObject loginText, passwordText;
    private Socket socket;
    private bool logged = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onLoginClicked);
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();
        socket.On("auth", () => {
            Debug.Log("Got auth event.");
            logged = true;
        });

    }

    //super-kostyl
    void Update() {
        if (logged)
        {
            Debug.Log("Changing scene");
            SceneManager.LoadScene("Game");
        }
    }

    void onLoginClicked()
    {
        string login = loginText.GetComponent<Text>().text;
        string password = passwordText.GetComponent<Text>().text;
        string accessToken;
        
        socket.Emit("login", "{\"login\": \"" + login + "\", \"password\": \"" + password + "\"}");
        socket.On("login", (data) =>
        {
            accessToken = (string)data;
            socket.Emit("auth", "{\"accessToken\": \"" + accessToken + "\"}");
        });
    }

}

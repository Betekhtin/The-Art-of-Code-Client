using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class Login : MonoBehaviour {

    public GameObject loginText, passwordText;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onLoginClicked);
    }

    void onLoginClicked()
    {
        string login = loginText.GetComponent<Text>().text;
        string password = passwordText.GetComponent<Text>().text;
        string accessToken;

        var socket = IO.Socket("http://91.225.131.223:8080");
        socket.On(Socket.EVENT_CONNECT, () =>
        {
            socket.Emit("login", "{\"login\": \"" + login + "\", \"password\": \"" + password + "\"}");
            Debug.Log("Connected");
        });
        socket.On("login", (data) =>
        {
            accessToken = (string)data;
            socket.Emit("auth", "{\"accessToken\": \"" + accessToken + "\"}");
            Debug.Log("Auth");
        });
    }

}

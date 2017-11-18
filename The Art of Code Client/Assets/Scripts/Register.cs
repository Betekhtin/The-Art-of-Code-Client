using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class Register: MonoBehaviour
{

    public GameObject loginText, passwordText, repeatPasswordText, nicknameText;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onLoginClicked);
    }

    void onLoginClicked()
    {
        string login = loginText.GetComponent<Text>().text;
        string password = passwordText.GetComponent<Text>().text;
        string passwordRepeat = repeatPasswordText.GetComponent<Text>().text;
        string nickname = nicknameText.GetComponent<Text>().text;
        string accessToken;

        if (password != passwordRepeat)
        {
            return;
        }

        var socket = IO.Socket("http://91.225.131.223:8080");
        socket.On(Socket.EVENT_CONNECT, () =>
        {
            socket.Emit("register", "{\"login\": \"" + login + "\", \"password\": \"" + password + "\", \"nickname\": \"" + nickname + "\"}");
            Debug.Log("Connected");
        });
        socket.On("register", (data) =>
        {
            accessToken = (string)data;
            socket.Emit("auth", "{\"accessToken\": \"" + accessToken + "\"}");
            Debug.Log("Auth");
        });
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.SceneManagement;

public class Register: MonoBehaviour
{
    public GameObject loginText, passwordText, repeatPasswordText, nicknameText;
    private Socket socket;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onLoginClicked);
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();
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
        
        socket.Emit("register", "{\"login\": \"" + login + "\", \"password\": \"" + password + "\", \"nickname\": \"" + nickname + "\"}");
        socket.On("register", (data) =>
        {
            accessToken = (string)data;
            socket.Emit("auth", "{\"accessToken\": \"" + accessToken + "\"}");
            socket.On("auth", () => { SceneManager.LoadScene("Game"); });
        });
    }

}

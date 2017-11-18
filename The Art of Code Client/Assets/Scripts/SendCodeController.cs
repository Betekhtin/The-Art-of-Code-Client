using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class SendCodeController : MonoBehaviour {

    public GameObject codeText;
    private Socket socket;

	void Start () {
        GetComponent<Button>().onClick.AddListener(onRunPressed);
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();

    }
	
	void onRunPressed() {
        string text = codeText.GetComponent<InputField>().text;
        socket.Emit("code", text);
    }

}

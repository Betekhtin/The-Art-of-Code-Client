﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterButtonController : MonoBehaviour {

	void Start () {
        GetComponent<Button>().onClick.AddListener(onRegisterClicked);
    }

    void onRegisterClicked()
    {
        SceneManager.LoadScene("RegisterScreen");
    }
	
}

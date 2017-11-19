using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeWrapper : MonoBehaviour {

    public GameObject codePanel;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(wrap);
    }

    void wrap()
    {
        codePanel.SetActive(!codePanel.activeSelf);
    }

}

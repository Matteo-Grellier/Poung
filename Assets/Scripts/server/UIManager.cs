using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField ipField;
    public Text ipText;

    private void Awake()   // à l'instenciation
    {
        if (instance == null)   // si y'a pas d'instance dit que celle ci est l'instance
        {
            instance = this;
        }
        else if (instance != this)  // sinon détruit l'instance comme ça y'a jamais 2 instances en même temps
        {
            Debug.Log("instance already exists, destroying object !");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        if (ipText.text != "")
        {
            Client.instance.ip = ipText.text;
        }
        startMenu.SetActive(false);
        usernameField.interactable = false;
        ipField.interactable = false;
        Client.instance.ConnectToServer();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineWin : GameManager
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(VictoryText);
        GetComponent<TextMeshProUGUI>().text = VictoryText;
    }
    public void BacktoMenu()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineWin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(VictoryMsg.msg);
        GetComponent<TextMeshProUGUI>().text = VictoryMsg.msg;
    }
    public void BacktoMenu()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
}

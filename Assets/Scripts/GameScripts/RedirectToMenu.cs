using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedirectToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GoBackHome").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

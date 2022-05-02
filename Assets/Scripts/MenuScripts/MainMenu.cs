using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGamevsBot()
    {
        SceneManager.LoadScene("PoungGameAI");
    }
    public void PlayGamevsPlayers()
    {
        SceneManager.LoadScene("PoungGame2players");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Join()
    {
        SceneManager.LoadScene("PoungOnline");
    }
}

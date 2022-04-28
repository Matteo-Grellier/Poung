using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Scene loadAIGame;
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
        loadAIGame = SceneManager.GetSceneByName("PoungGame2players");
        SceneManager.LoadScene("PoungGame2players");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

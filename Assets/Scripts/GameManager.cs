using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    #region réseau 

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    #endregion

    public bool player1HasWin;
    public bool gameHasStarted = false;
    [Header("Particles")]
    public ParticleSystem ballTrail;
    public BallControl ballController;
    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    // public GameObject player1Paddle;
    public GameObject player1Goal;
    public int paddleSpeed;
    [Header("AI")]
    // public GameObject player2Paddle;
    public GameObject player2Goal;
    public float aiSpeed;

 
    [Header("Score UI")]
    public GameObject Player1Text;
    public GameObject Player2Text;

    public int Player1Score;
    public int Player2Score;

    public static string VictoryText;
    public void EndGame()
    {
        if (Player1Score == 5)
        {
            SceneManager.LoadScene("VictoryOnline");
            VictoryText = "Player 1 Wins!";
        }
        else if (Player2Score == 5)
        {
            SceneManager.LoadScene("VictoryOnline");
            VictoryText = "Player 2 Wins!";
        }
    }

    public void Player1Scored()
    {
        Debug.Log("Player 1 Scored");
        Player1Score++;
        
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
    }

    public void Player2Scored()
    {
        Player2Score++;
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
    }

    private void Update() 
    {

        EndGame();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Player2Score = 5;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Reset();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            Reset();
        }
    }

    public void Launch()
    {
        ballController.ShotBall();
    }

    public void Reset()
    {
        ballTrail.Stop();
        ballController.ResetAllPositions();
    }

    private void Start() 
    {
        ballController = ball.GetComponent<BallControl>();
        ballTrail.Stop();

        // stop particles from playing
        if (SceneManager.GetActiveScene().name != "PoungOnline")
        {
            Launch();
        }

        ballController = ball.GetComponent<BallControl>();
    }

    #region réseau

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

    public void SpawnPlayer(int _id, string _username, Vector3 _position)
    {
        GameObject _player;
        if (_id == Client.instance.myId) // if is local player
        {
            _player = Instantiate(localPlayerPrefab, _position, new Quaternion(0, 0, 0, 0) );
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, new Quaternion(0, 0, 0, 0) );
            Launch();
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    #endregion
}

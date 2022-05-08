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
    public float maxSpeed = 15f;
    public GameObject ball;

    [Header("Player 1")]
    public GameObject player1Paddle;
    public GameObject player1Goal;
    public int paddleSpeed;
    [Header("AI")]
    public GameObject player2Paddle;
    public GameObject player2Goal;
    public float aiSpeed;


    [Header("Score UI")]
    public GameObject Player1Text;
    public GameObject Player2Text;

    public int Player1Score;
    public int Player2Score;

    public void EndGame()
    {
        if (Player1Score == 5)
        {
            if (SceneManager.GetActiveScene().name != "PoungOnline" && SceneManager.GetActiveScene().name != "VictoryOnline")
            {
                SceneManager.LoadScene("VictoryOnline");
                VictoryText = "Player 1 Wins!";
            }
        }
        else if (Player2Score == 5)
        {
            if (SceneManager.GetActiveScene().name != "PoungOnline" && SceneManager.GetActiveScene().name != "VictoryOnline")
            {
                SceneManager.LoadScene("VictoryOnline");
                VictoryText = "Player 2 Wins!";
            }
        }
    }

    public void Player1Scored()
    {
        Debug.Log("Player 1 Scored");
        Player1Score++;

        ClientSend.SendPointScored(1);
        
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
    }

    public void Player2Scored()
    {
        Debug.Log("Player 1 Scored");
        Player2Score++;

        ClientSend.SendPointScored(2);

        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
    }

    public void SetScore(int _scoreP1, int _scoreP2)
    {
        Player1Score = _scoreP1;
        Player2Score = _scoreP2;
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
    }

    private void Update() 
    {
        EndGame();
        if (Input.GetKeyDown(KeyCode.Escape)){
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
    public void Launch(int _sideToLaunchTo) // to replace the one above
    {
        ballController.ShotBall(_sideToLaunchTo);
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
        ballController.ResetAllPositions();

        // no need for launch in local beacause resetAllPosition as a launch
        // if (SceneManager.GetActiveScene().name != "PoungOnline")
        // {
        //     Launch();
        // }

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

    public void LaunchWin(int _winingPlayer)
    {
        if (_winingPlayer == Client.instance.myId) // if is local player
        {
            // WiningScreen();
            SceneManager.LoadScene("VictoryOnline");
            VictoryText = "You won !";
        }
        else
        {
            // LoosingScreen();
            SceneManager.LoadScene("VictoryOnline");
            VictoryText = "You lost ...";
        }   
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, int _sideToLaunchBall)
    {
        GameObject _player;
        if (_id == Client.instance.myId) // if is local player
        {
            _player = Instantiate(localPlayerPrefab, _position, new Quaternion(0, 0, 0, 0) );
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, new Quaternion(0, 0, 0, 0) );
            Launch(_sideToLaunchBall);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
    #endregion
}

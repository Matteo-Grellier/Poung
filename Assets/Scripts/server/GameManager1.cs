using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    //---------------------------------------------------vas y'avoir des erreurs parce que pour l'instant il s'apelle pas game manager-------
    public static GameManager1 instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;


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
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
}

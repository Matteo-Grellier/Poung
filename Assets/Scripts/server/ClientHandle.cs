using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();


        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        int _sideToLaunchBall = _packet.ReadInt();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _sideToLaunchBall);
    }

    public static void LaunchGame(Packet _packet)
    {
        int _sideToLaunchTo = _packet.ReadInt();
        int _scoreP1 = _packet.ReadInt();
        int _scoreP2 = _packet.ReadInt();

        GameManager.instance.SetScore(_scoreP1, _scoreP2);
        GameManager.instance.Launch(_sideToLaunchTo);
    }

    public static void Winning(Packet _packet)
    {
        int _winningPlayer = _packet.ReadInt();

        GameManager.instance.LaunchWin(_winningPlayer);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }
}

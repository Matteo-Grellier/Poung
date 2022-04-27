using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;


public class Client : MonoBehaviour
{
    public static Client instance;  // instance
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;

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

    private void Start()
    {
        tcp = new TCP();
    }

    public void ConnectToServer()
    {
        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize, // donne a taille du buffer
                SendBufferSize = dataBufferSize     // donne a taille du buffer
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLenght = stream.EndRead(_result); // Attend que la requête asynchrone en attente se termine
                if ( _byteLenght <= 0) // si "_byteLenght" est vide : disconnect
                {
                    // TODO : disconnect
                    return;
                }

                byte[] _data = new byte[_byteLenght];
                Array.Copy(receiveBuffer, _data, _byteLenght);  // met les infos reçus dans "_data"

                // TODO : handle data

                // Débute une opération de lecture asynchrone et apelle "ReceiveCallback" quand l'opération est terminée ( recommence quoi )
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            }
            catch
            {
                // TODO: disconnect
            }
        }
    }

}

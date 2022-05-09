using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace PoungServer
{
    internal class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public Player player;
        public TCP tcp;
        public UDP udp;

        public Client(int _clientId) // constructeur de Client
        {
            id = _clientId;
            tcp = new TCP(id);  // créé un nouveau TCP à l'instenciation
            udp = new UDP(id);  // créé un nouveau UDP à l'instenciation
        }

        public class TCP
        {
            public TcpClient socket; // la connexion au client ( vide à l'instenciation de TCP )

            private readonly int id;
            private NetworkStream stream;
            private Packet receiveData;
            private byte[] receiveBuffer;

            public TCP(int _id) // constructeur de TCP
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;   // renseigne le socket
                socket.ReceiveBufferSize = dataBufferSize;  // renseigne la taille du buffer
                socket.SendBufferSize = dataBufferSize;     // renseigne la taille du buffer

                stream = socket.GetStream();    // choppe le stream

                receiveData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                // Débute une opération de lecture asynchrone et apelle "ReceiveCallback" quand l'opération est terminée
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                
                ServerSend.Welcome(id, "Welcome to the server !");
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP : {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                // // protection contre le client vide
                // if (Server.clients.Count != 0)
                // {
                    try
                    {
                        int _byteLenght = stream.EndRead(_result); // Attend que la requête asynchrone en attente se termine
                        if ( _byteLenght <= 0) // si "_byteLenght" est vide : disconnect
                        {
                            Server.clients[id].Disconnect();
                            return;
                        }

                        byte[] _data = new byte[_byteLenght];
                        Array.Copy(receiveBuffer, _data, _byteLenght);  // met les infos reçus dans "_data"

                        receiveData.Reset(HandleData(_data));

                        // Débute une opération de lecture asynchrone et apelle "ReceiveCallback" quand l'opération est terminée ( recommence quoi )
                        stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                    }
                    catch (Exception _ex)
                    {
                        Console.WriteLine($"Error, receiving TCP data : {_ex}");
                        Server.clients[id].Disconnect();

                    }
                // }
            }


            private bool HandleData(byte[] _data)
            {
                int _packetLenght = 0;

                receiveData.SetBytes(_data);

                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLenght = receiveData.ReadInt();
                    if (_packetLenght <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLenght > 0 && _packetLenght <= receiveData.UnreadLength())
                {
                    byte[] _packetBytes = receiveData.ReadBytes(_packetLenght);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Server.packetHandlers[_packetId](id, _packet);
                        }
                    });

                    _packetLenght = 0;

                    if (receiveData.UnreadLength() >= 4)
                    {
                        _packetLenght = receiveData.ReadInt();
                        if (_packetLenght <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLenght <= 1)
                {
                    return true;
                }

                return false;
            }

            public void Disconnect()
            {
                socket.Close();
                stream = null;
                receiveData = null;
                receiveBuffer = null;
                socket = null;
            }

        }

        public class UDP
        {
            public IPEndPoint endPoint;

            private int id;

            public UDP(int _id)
            {
                id = _id;
            }

            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;
            }

            public void SendData(Packet _packet)
            {
                Server.SendUDPData(endPoint, _packet);
            }

            public void HandleData(Packet _packetData)
            {
                int _packetLength = _packetData.ReadInt();
                byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId =  _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });
            }

            public void Disconnect()
            {
                endPoint = null;
            }
        }

        public void SendIntoGame(string _playerName) 
        {
            if ( id == 1)
            {
                player = new Player(id, _playerName, new Vector3(-8.27f, 0, 0)); // player 1 donc à gauche
            }
            else
            {
                player = new Player(id, _playerName, new Vector3(8.27f, 0, 0)); // player 2 donc à droite
            }

            foreach (Client _client in Server.clients.Values) // "récupère de tous les joeurs"
            {
                if (_client.player != null)
                {
                    if (_client.id != id)
                    {
                        ServerSend.SpawnPlayer(id, _client.player); // et envoie au joueur les infos de tous les autres players
                    }
                }
            }

            foreach (Client _client in Server.clients.Values) // envoie des infos a tous les autres players (et à lui même)
            {
                if (_client.player != null)
                {
                    ServerSend.SpawnPlayer(_client.id, player); // et envoie au joueur les infos de tous les autres players
                }
            }
        }

        public void SendLaunchGame(int _LastPlayerToScore)
        {
            int point;
            if (_LastPlayerToScore == 1)
            {
                point = 1;
            } 
            else if (_LastPlayerToScore == 2)
            {
                point = -1;
            }
            else
            {
                Random rnd = new Random();
                point  = rnd.Next(0, 2) == 0 ? 1 : -1; // entre 0 et 1 et si 0 devient -1
            }

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    ServerSend.SendBall(_client.id, point);
                }
            }
        }

        private void Disconnect()
        {
            Console.WriteLine($"{tcp.socket.Client.RemoteEndPoint} has diconnected");

            if (GameLogic.numberOfPlayerConnected > 0)
            { 
                GameLogic.numberOfPlayerConnected--;
                Console.WriteLine($" numberOfPlayerConnected = {GameLogic.numberOfPlayerConnected}");
            }

            //SendWin()

            player = null;

            tcp.Disconnect();
            udp.Disconnect();
        }
    }
}

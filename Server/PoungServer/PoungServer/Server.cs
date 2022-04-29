using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace PoungServer
{
    internal class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        // liste de tous les joueurs
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _maxPlayer, int _port )
        {
            MaxPlayers = _maxPlayer;
            Port = _port;

            Console.WriteLine($"Server starting ...");
            InitializeServerData(); // voir plus bas

            tcpListener = new TcpListener(IPAddress.Any, Port); // le TcpListener doit écouter toutes les adresses IP sur le port "PORT"
            tcpListener.Start(); // lance le TcpListener

            // demarre une opération asynchrone pour accepter une tentative de connexion entrante et apelle "TCPConnectCallback" quand l'opération est terminée
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null );

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"server started successfully on port {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            // quand une tentative de connexion réussi, renseigne le cleint dans un nouveau TcpClient
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);

            // Démarre une opération asynchrone pour accepter une tentative de connexion entrante, apelle "TCPConnectCallback" quand l'opération est terminée ( recommence quoi )
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++) // pour chaques "joueurs"
            {
                if (clients[i].tcp.socket == null)   // si la proptiété socket dans leur TCP est null
                {
                    clients[i].tcp.Connect(_client);    // on connect le client
                    return;                             // et on quitte la fonction
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect : Server full !");    // sinon erreur
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try 
            { 
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive( UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString() )
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData() // créé une instance pour chaque joueurs en fonction du nombre max de joueurs
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
            };
            Console.WriteLine("Initialized packets.");
        }
    }
}

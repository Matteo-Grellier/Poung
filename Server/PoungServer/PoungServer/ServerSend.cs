using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PoungServer
{
    internal class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendTCPData()");

            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendUDPData()");

            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendTCPDataToAll()");

            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendTCPDataToAll()");

            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if ( i != _exceptClient) 
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendUDPDataToAll()");

            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            Console.WriteLine($"() ServerSend.cs SendUDPDataToAll()");

            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg) 
        {
            Console.WriteLine($"() ServerSend.cs Welcome()");

            // using dispose du packet une fois qu'on a finit de l'utiliser ( car packet hérite de disposable )
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            Console.WriteLine($"() ServerSend.cs SpawnPlayer()");

            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer)) // créé un packet spawnPlayer
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                Random rnd = new Random();
                _packet.Write( rnd.Next(0, 2) == 0 ? 1 : -1 );

                SendTCPData(_toClient, _packet); // on utilise tcp parce que ça arrive qu'une fois et dcp on veut pas perdre le packet
            }
        }

        public static void SendBall(int _toClient, int _point)
        {
            Console.WriteLine($"() ServerSend.cs SendBall()");

            using (Packet _packet = new Packet((int)ServerPackets.sendBallLaunch)) // créé un packet sendBallLaunch
            {
                _packet.Write(_point);
                _packet.Write(GameLogic.scoreP1);
                _packet.Write(GameLogic.scoreP2);

                SendTCPData(_toClient, _packet); // on utilise tcp parce que ça arrive qu'une fois et dcp on veut pas perdre le packet
            }
        }

        public static void SendWin(int _winingPlayer)
        {
            Console.WriteLine($"() ServerSend.cs SendWin()");

            using (Packet _packet = new Packet((int)ServerPackets.sendWin))
            {
                _packet.Write(_winingPlayer);

                SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            Console.WriteLine($"() ServerSend.cs PlayerPosition()");

            using (Packet _packet = new Packet((int)ServerPackets.playerPosition)) 
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);

                SendUDPDataToAll(_packet); 
            }
        }
        #endregion
    }
}

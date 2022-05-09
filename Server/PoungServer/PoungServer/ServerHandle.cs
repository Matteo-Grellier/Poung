using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PoungServer
{
    internal class ServerHandle
    {
        // to see how many response of scoring where received
        private static int scoringPacketReiceived = 0;
        private static int lastScoringPlayer = 0;

        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            GameLogic.numberOfPlayerConnected++;
            Console.WriteLine($" numberOfPlayerConnected = {GameLogic.numberOfPlayerConnected}");

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            Console.WriteLine($" TEST connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username); // spawn le player
        }

        public static void PointScoredReceived(int _fromClient, Packet _packet)
        {
            int _idOfScoringPlayer = _packet.ReadInt();

            Console.WriteLine($" client {_fromClient} says that the player {_idOfScoringPlayer} scored !");

            if (scoringPacketReiceived == 0) // si on a déja reçu un scoring packet on le met pas
            { 
                lastScoringPlayer = _idOfScoringPlayer;
            }   

            scoringPacketReiceived++;

            if (scoringPacketReiceived == 2)
            {
                int goodPlayerToSend = 0;

                if (_idOfScoringPlayer == lastScoringPlayer)
                {
                    goodPlayerToSend = _idOfScoringPlayer;

                    if (_idOfScoringPlayer == 1)
                    {
                        GameLogic.scoreP1++;
                    } 
                    else if (_idOfScoringPlayer == 2)
                    {
                        GameLogic.scoreP2++;
                    }
                }

                scoringPacketReiceived = 0;
                scoringPacketReiceived = 0;

                Server.clients[_fromClient].SendLaunchGame(goodPlayerToSend);
            }
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }

            // pour eviter le crash
            if (GameLogic.numberOfPlayerConnected == 1 || GameLogic.numberOfPlayerConnected == 2)
            { 
                Server.clients[_fromClient].player.SetInput(_inputs);
            }
        }

    }
}

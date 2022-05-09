using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoungServer
{
    internal class GameLogic
    {
        public static int scoreP1 = 0;
        public static int scoreP2 = 0;

        public static int numberOfPlayerConnected = 0;

        public static void Update()
        {

            // // protection contre le clients null après win
            // Console.WriteLine($"###: Gamemanager verif of clients count  = {Server.clients.Count}");
            // if (Server.clients.Count != 0)
            // {

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }

            if (scoreP1 == 5)
            {
                ServerSend.SendWin(1);
                scoreP1 = 0;
                scoreP2 = 0;
                numberOfPlayerConnected = 0;
                Console.WriteLine($" numberOfPlayerConnected = {GameLogic.numberOfPlayerConnected}");
                 Server.clients.Clear();
            }
            else if (scoreP2 == 5)
            {
                ServerSend.SendWin(2);
                scoreP1 = 0;
                scoreP2 = 0;
                numberOfPlayerConnected = 0;
                Console.WriteLine($" numberOfPlayerConnected = {GameLogic.numberOfPlayerConnected}");
                 Server.clients.Clear();
            }

            // }

            ThreadManager.UpdateMain();
        }
    }
}

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

        public static void Update()
        {
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }

                if (scoreP1 == 5)
                {
                    ServerSend.SendWin(1);
                }
                else if (scoreP2 == 5)
                {
                    ServerSend.SendWin(2);
                }
            }

            ThreadManager.UpdateMain();
        }
    }
}

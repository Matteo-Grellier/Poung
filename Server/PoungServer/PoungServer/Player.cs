using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PoungServer
{
    internal class Player
    {
        public int id;
        public string username;

        public Vector3 position;

        public Player(int _id, string _username, Vector3 _position) 
        {
            id = _id;
            username = _username;
            position = _position;
        }
    }
}

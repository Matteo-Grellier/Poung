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

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public Player(int _id, string _username, Vector3 _position) 
        {
            id = _id;
            username = _username;
            position = _position;

            inputs = new bool[2];
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if (inputs[0]) 
            {
                if (position.Y < 4)
                {
                    _inputDirection.Y += 1;
                }
            }
            if (inputs[1])
            {
                if (position.Y > -4)
                {
                    _inputDirection.Y -= 1;
                }
            }

            Move(_inputDirection);
        }

        private void Move(Vector2 _inputDirection)
        {
            Vector3 _moveDirection = new Vector3(0, _inputDirection.Y, 0);

            position += _moveDirection * moveSpeed;
            //position = new Vector3(position.X, position.Y + _moveDirection.Y , position.Z);


            ServerSend.PlayerPosition(this);
        }

        public void SetInput(bool[] _inputs)
        {
            inputs = _inputs;
        }
    }
}

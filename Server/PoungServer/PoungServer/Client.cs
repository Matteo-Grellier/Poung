using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace PoungServer
{
    internal class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;

        public Client(int _clientId) // constructeur de Client
        {
            id = _clientId;
            tcp = new TCP(id);  // créé un nouveau TCP à l'instenciation
        }

        public class TCP
        {
            public TcpClient socket; // la connexion au client ( vide à l'instenciation de TCP )

            private readonly int id;
            private NetworkStream stream;
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

                receiveBuffer = new byte[dataBufferSize];

                // Débute une opération de lecture asynchrone et apelle "ReceiveCallback" quand l'opération est terminée
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                // TODO : send welcome package
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
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error, receiving TCP data : {_ex}");
                    // TODO : disconnect

                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer() // on envoie les inputs au server et pas la position sinon il pourrait envoyer ce qu'il veut et cheater
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.Z),
            Input.GetKey(KeyCode.S),
        };

        ClientSend.PlayerMovement(_inputs);
    }
}

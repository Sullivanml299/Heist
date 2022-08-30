using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CreateCharacterMessage : NetworkMessage
{
    public string name;
    public Color color;
}

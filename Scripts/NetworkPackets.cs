using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerPackets
{
    SServerFull = -1,
    SConnectionOK = 1,
    SMove = 2
}

public enum ClientPackets
{
    CServerFull = -1,
    CThankyou = 1,
    CMove = 2
}
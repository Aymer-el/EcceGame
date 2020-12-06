using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandleNetworkData: MonoBehaviour
{
        private delegate void Packet_(byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Debug.Log("Initialize network packages.");
            Packets = new Dictionary<int, Packet_>
            {
                { (int)ServerPackets.SServerFull, handleServerFull },
                { (int)ServerPackets.SConnectionOK, handleConnectionOK },
                { (int)ServerPackets.SMove, handleSMove }
            };
        }

        public void Awake()
        {
        Debug.Log("in awake");
            InitializeNetworkPackages();
        }
    public static void HandleNetworkInformation(byte[] data)
    {
        int packetnum; PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        packetnum = buffer.ReadInteger();
        buffer.Dispose();
        if (Packets.TryGetValue(packetnum, out Packet_ Packet))
        {
            Packet.Invoke(data);
        }
    }
    private static void handleConnectionOK(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            ClientTCP.testmsg = msg;
            Debug.Log(msg);

        // add your code you want to execute here:
        ClientTCP.ThankYouServer();
        }

    private static void handleSMove(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        Debug.Log(msg);
        buffer.Dispose();
    }

    private static void handleServerFull(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        Debug.Log(msg);
        buffer.Dispose();
    }
}

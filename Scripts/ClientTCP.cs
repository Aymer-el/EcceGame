using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class ClientTCP : MonoBehaviour
{
    // Start is called before the first frame update
    public static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _asyncbuffer = new byte[1024];
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        _clientSocket.BeginConnect("127.0.0.1", 6321, new AsyncCallback(ConnectCallBack), _clientSocket);
        ClientHandleNetworkData.InitializeNetworkPackages();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ConnectCallBack(IAsyncResult ar)
    {
        _clientSocket.EndConnect(ar);
        while (true)
        {
            OnReceive();
            Debug.Log("on receive");
        }
    }

    public static void OnReceive()
    {
        byte[] _sizeinfo = new byte[4];
        byte[] _receivedbuffer = new byte[1024];

        int totalread = 0, currentread = 0;

        try
        {
            currentread = totalread = _clientSocket.Receive(_sizeinfo);
            if (totalread <= 0)
            {
                Console.WriteLine("You are not connected to the server.");
            }
            else
            {
                Console.WriteLine("Receiving", _receivedbuffer);
                while (totalread < _sizeinfo.Length && currentread > 0)
                {
                    currentread = _clientSocket.Receive(_sizeinfo, totalread, _sizeinfo.Length - totalread, SocketFlags.None);
                    totalread += currentread;
                }

                int messagesize = 0;
                messagesize |= _sizeinfo[0];
                messagesize |= (_sizeinfo[1] << 8);
                messagesize |= (_sizeinfo[2] << 16);
                messagesize |= (_sizeinfo[3] << 24);

                byte[] data = new byte[messagesize];

                totalread = 0;
                currentread = totalread = _clientSocket.Receive(data, totalread, data.Length - totalread, SocketFlags.None);

                while (totalread < messagesize && currentread > 0)
                {
                    currentread = _clientSocket.Receive(data, totalread, data.Length - totalread, SocketFlags.None);
                    totalread += currentread;
                }

                ClientHandleNetworkData.HandleNetworkInformation(data);
                //HandleNetworkInformation
            }

        }
        catch
        {
            Console.WriteLine("You are not connected to the server.");
        }
    }
    public static void SendData(byte[] data)
    {
        _clientSocket.Send(data);
    }
    public static void ThankYouServer()
    {
        Debug.Log("in thanks you");
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CThankyou);
        buffer.WriteString("Thanks you server");
        SendData(buffer.ToArray());
        buffer.Dispose();
    }

    public static void SendCMove(string msg)
    {
        Debug.Log("in thanks you");
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CMove);
        buffer.WriteString(msg);
        SendData(buffer.ToArray());
        buffer.Dispose();
    }

}

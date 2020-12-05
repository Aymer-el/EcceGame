using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;



public class ClientTCP : MonoBehaviour
{
    // Start is called before the first frame update
    public static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _asyncbuffer = new byte[1024];

    public static int players = 0;
    public static int roomNumber;
    public static String testmsg = "";
    public static bool connected;

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
        Debug.Log(testmsg);

        if (testmsg != "")
        {
            string[] aData = testmsg.Split('|');
            Int32.TryParse(aData[2], out int numberOfPlayer);
            Debug.Log(testmsg);
            switch (aData[0])
            {
                case "SWHO":
                    for (int i = 1; i < aData.Length - 1; i++)
                    {
                        ClientTCP.roomNumber = int.Parse(aData[1]);
                        ClientTCP.UserConnnected(int.Parse(aData[2]));
                    }
                    break;
                case "SCNN":
                    break;
                case "SMOV":
                    //Global.EcceInstance.TrySelectPiece(new Vector2(int.Parse(aData[1]), int.Parse(aData[2])), int.Parse(aData[5]));
                    Global.EcceInstance.TryMovePiece(
                        new Vector2(int.Parse(aData[4]), int.Parse(aData[5])),
                        new Vector2(int.Parse(aData[6]), int.Parse(aData[7])));
                    break;
                case "SPLA":
                    try
                    {
                        Global.EcceInstance.TryPlaceNewPiece(int.Parse(aData[3]));
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    break;
                case "SSEL":
                    Global.EcceInstance.TrySelectPiece(
                        new Vector2(int.Parse(aData[4]), int.Parse(aData[5])),
                        int.Parse(aData[3]));
                    break;
            }
            testmsg = "";
        }
    }

    public static void ConnectCallBack(IAsyncResult ar)
    {
        _clientSocket.EndConnect(ar);
        connected = true;
        while (true)
        {
            OnReceive();
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
                Debug.Log("You are not connected to the server.");
            }
            else
            {
                Debug.Log(_receivedbuffer);
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
            Debug.Log("You are not connected to the server.");
        }
    }
    public static void SendData(byte[] data)
    {
        _clientSocket.Send(data);
    }
    public static void ThankYouServer()
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CThankyou);
        buffer.WriteString("Thanks you server");
        SendData(buffer.ToArray());
        buffer.Dispose();
    }

    public static void SendCMove(string msg)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CMove);
        buffer.WriteString(msg);
        SendData(buffer.ToArray());
        buffer.Dispose();
    }
    public static void UserConnnected(int p)
    {
        players = p;
        if(players == 2)
        {
            GameManager.StartGame();
        }
    }


}

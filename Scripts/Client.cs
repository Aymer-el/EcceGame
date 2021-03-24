using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.IO;
using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public bool isHost = false;


    private List<GameClient> players = new List<GameClient>();

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
        {
            return false;
        }

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        } catch(Exception e)
        {
            Debug.Log("socket error " + e.Message);
        }

        return socketReady;
    }

    // Read messages from the server
    private void OnIncomingData(string data)
    {
        string[] aData = data.Split('|');
        switch (aData[0])
        {
            case "SWHO":
                for(int i = 1; i < aData.Length -1; i++)
                {
                    UserConnnected(aData[i], false);
                }
                Send("CWHO|" + clientName + "|" + ((isHost)?1:0).ToString());
                break;
            case "SCNN":
                UserConnnected(aData[1], false);
                break;
            case "SMOV":
                Global.EcceInstance.TrySelectPiece(new Vector2(int.Parse(aData[1]), int.Parse(aData[2])), int.Parse(aData[5]));
                Global.EcceInstance.TryMovePiece(
                    new Vector2(int.Parse(aData[2]), int.Parse(aData[3])),
                    new Vector2(int.Parse(aData[4]), int.Parse(aData[5])));
                break;
            case "SPLA":
                Global.EcceInstance.TryPlaceNewPiece(int.Parse(aData[3]));
                break;
            case "SSEL":
                Global.EcceInstance.TrySelectPiece(
                    new Vector2(int.Parse(aData[4]), int.Parse(aData[5])),
                    int.Parse(aData[3]));
                break;
        }
    }

    private void UserConnnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;
        players.Add(c);
        Debug.Log(players.Count);
        if(players.Count == 2)
        {
            GameManager.StartGame();
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    public void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    public void Send(string data)
    {
        if (!socketReady)
        {
            return;
        }

        writer.WriteLine(data);
        writer.Flush();
    }

    public void CloseSocket()
    {
        if (!socketReady)
        {
            return;
        }
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}

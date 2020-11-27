using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;

public class Server : MonoBehaviour
{
    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnnectList;

    public TcpListener server;
    private bool serverStarted;

    public void Init() {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnnectList = new List<ServerClient>();
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }catch(Exception e)
        {
            Debug.Log("socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if(!serverStarted)
            return;
        foreach(ServerClient c in clients)
        {
            // Is the client still connected
            if (!IsConnected(c.tcp))
            {
                Debug.Log("disconnected");
                c.tcp.Close();
                disconnnectList.Add(c);
                continue;
            } else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    Debug.Log(data);

                    if(data != null)
                    {
                        OnInComingData(c, data);
                    }
                }
            }
        }


        for(int i = 0; i < disconnnectList.Count -1; i++)
        {
            // Tell our player somebody has disconnected
            clients.Remove(disconnnectList[i]);
            disconnnectList.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    // Remember who client is
    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient i in clients)
        {
            allUsers += i.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        Debug.Log("somebody has connected !");

        StartListening();
        Broadcast("SWHO|" + allUsers, clients[clients.Count - 1]);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if(c != null && c.Client != null && c.Client.Connected)
            {
                if(c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            } else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    // Server Send
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach(ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            } catch(Exception e)
            {
                Debug.Log("write error : " + e.Message);
            }
        }
    }

    private void Broadcast(string data, ServerClient cl)
    {
        List<ServerClient> sc = new List<ServerClient> { cl };
        Broadcast(data, sc);
    }
    // Server Read
    private void OnInComingData(ServerClient c, string data)
    {
        Debug.Log("Server:" + data);
        string[] aData = data.Split('|');
        switch (aData[0])
        {
            case "CWHO":
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true;
                Broadcast("SCNN|" + c.clientName, clients);
                break;
            case "CMOV":
                data.Replace("C", "S");
                Broadcast(data, clients);
                break;
        }
    }
}

public class ServerClient
{
    public string clientName;
    public bool isHost;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ButtonConnect;
    public GameObject ButtonHost;
    public GameObject ButtonBack;
    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;
    public GameObject ServerPrefab;
    public GameObject ClientPrefab;
    public static bool isActive = true;

    public static GameManager Instance { set; get; }
    // Start is called before the first frame update
    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        connectMenu.SetActive(false);

        Host(() => {
            try
            {
                Server s = Instantiate(ServerPrefab).GetComponent<Server>();
                s.Init();

                Client c = Instantiate(ClientPrefab).GetComponent<Client>();
                c.isHost = true;
                if(c.clientName == "")
                {
                    c.clientName = "Host";
                }
                c.ConnectToServer("127.0.0.1", 6321);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        });
        Back(() => {
            Global.WinnerInt = -1;
            SceneManager.LoadScene("MultiplayerScene");
        });
        Connect(() => {
            string hostAddress = GameObject.Find("HostInput").GetComponent<TMP_InputField>().text;
            if(hostAddress == "")
            {
                hostAddress = "127.0.0.1";
            }

            try
            {
                Client c = Instantiate(ClientPrefab).GetComponent<Client>();
                c.clientName = GameObject.Find("NameInput").GetComponent<TMP_InputField>().text;
                if(c.clientName == "")
                {
                    c.clientName = "client";
                }
                c.isHost = false;
                c.ConnectToServer(hostAddress, 6321);
                Debug.Log("by connect button");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        });
    }

    // Update is called once per frame
    public void Connect(UnityAction action)
    {
        ButtonConnect.GetComponentInChildren<Button>().onClick.AddListener(action);
    }

    public void Host(UnityAction action)
    {
        ButtonHost.GetComponentInChildren<Button>().onClick.AddListener(action);
    }

    public void Back(UnityAction action)
    {
        GameObject.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(action);
        //this.isActive = false;
    }

    public void StartGame()
    {
        GameManager.isActive = false;
        SceneManager.LoadScene("NewGameScene");
    }

    public void Update()
    {
        GameObject.Find("CanvasServerMenu").SetActive(GameManager.isActive);
    }
}
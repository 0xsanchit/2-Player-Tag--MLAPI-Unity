using System;
using System.Collections;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


public class Hosting : NetworkBehaviour
{

    public Transform HostPrefab;
    public Transform ClientPrefab;
    public GameObject startButton;
    private GameObject _host;
    private GameObject _client;
    public GameObject options;
    public GameObject main;
    public float MaximumRounds;
    public float MaximumTime;
    public TextMeshProUGUI TimeSelectedText;
    public TextMeshProUGUI RoundsSelectedText;
    public string ipv4;
    public TextMeshProUGUI IP;

    private void Start()
    {
        ipv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
        IP.text = ipv4;
        Debug.Log("IP"+ipv4);
        FindObjectOfType<AudioManager>().Play("BackGround");
        MaximumTime = 10;
        MaximumRounds = 1;
        if (IsHost)
        {
            SpawnHostServerRpc(NetworkManager.Singleton.LocalClientId);
            startButton.SetActive(true);
        }
        else if(IsClient)
        {
            Debug.Log("client");
            startButton.SetActive(false);
            SpawnClientServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            DestroyHostServerRpc();
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            DestroyClientServerRpc();
            NetworkManager.Singleton.StopClient();
        }
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        main.SetActive(false);
        options.SetActive(true);
        /*ConnectionHandle.AllowConnections = false;
        NetworkSceneManager.SwitchScene("Loading");*/
    }
    
    public void Back()
    {
        main.SetActive(true);
        options.SetActive(false);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnHostServerRpc(ulong clientId)
    {
/*        float x = -300 + 280 * ((clientId > 0) ? (int)clientId - 1 : 0);
*/        float x = -100;
        _host = Instantiate(HostPrefab, new Vector3(x, 13, 0), Quaternion.identity).gameObject;
        _host.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnClientServerRpc(ulong clientId)
    {
        Debug.Log("Spawning Client");
/*        float x = -300 + 280 * ((clientId > 0) ? (int)clientId - 1 : 0);
*/        float x = 100;
        _client = Instantiate(ClientPrefab, new Vector3(x, 15, 0), Quaternion.identity).gameObject;
        _client.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyHostServerRpc()
    {
        _host.GetComponent<NetworkObject>().Despawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyClientServerRpc()
    {
        _client.GetComponent<NetworkObject>().Despawn(true);
    }

    private void Update()
    {
        if(IsHost)
        {
            TimeSelectedText.text = MaximumTime.ToString() + "  Sec";
            RoundsSelectedText.text = MaximumRounds.ToString();
        }
        /*        if(!IsClient) SceneManager.LoadScene(0);
        */
    }

    public void LoadOptions()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void IncreaseRounds()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
        MaximumRounds = MaximumRounds + 1f;

    }

    public void DereaseRounds()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
        if (MaximumRounds >= 2)
            MaximumRounds = MaximumRounds - 1f;
    }

    public void IncreaseTime()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
        MaximumTime = MaximumTime + 5f;

    }

    public void DereaseTime()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
        if (MaximumTime >= 6)
            MaximumTime = MaximumTime - 5f;
    }

    public void StartGame()
    {
        FindObjectOfType<Values>().MaximumTime = MaximumTime;
        FindObjectOfType<Values>().MaximumRounds = MaximumRounds;
        FindObjectOfType<Values>().BlueScore = 0;
        FindObjectOfType<Values>().RedScore = 0;
        FindObjectOfType<AudioManager>().Play("Mouse Click");
        FindObjectOfType<Values>().currentRound = 1;
        ConnectionHandle.AllowConnections = false;
        NetworkSceneManager.SwitchScene("LevelOneNetwork");
    }

    public void OpenSettings()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
    }

    public void SinglePlayer()
    {
        FindObjectOfType<Values>().numPlayers = 1;
        LoadOptions();
    }

    public void TwoPlayer()
    {
        FindObjectOfType<Values>().numPlayers = 2;
        LoadOptions();
    }

    public void CloseSettings()
    {
        FindObjectOfType<AudioManager>().Play("Mouse Click");
    }


public class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (ip.Address.ToString() != "127.0.0.1")
                                {
                                    output = ip.Address.ToString();

                                }
                            }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }

}

    public enum ADDRESSFAM
{
    IPv4, IPv6
}

}
 
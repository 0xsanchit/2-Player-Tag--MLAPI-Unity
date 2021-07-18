using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using TMPro;
using MLAPI.NetworkVariable;



public class NetworkGameManager : NetworkBehaviour
{
    /*    public GameObject pausedMenu;
        public Transform player;*/
    public string nextLevel;

    public Transform HostPrefab;
    public Transform ClientPrefab;

    private GameObject _host;
    private GameObject _client;

    public Canvas RoundCompleteCanvas;
    public NetworkVariable<float> BlueScore = new NetworkVariable<float>();
    public NetworkVariable<float> RedScore = new NetworkVariable<float>();
    public TextMeshProUGUI RedScoreContainer;
    public TextMeshProUGUI BlueScoreContainer;
    /*    float CurrentTime;
    */
    public NetworkVariable<float> CurrentTime = new NetworkVariable<float>();

    public NetworkVariable<bool> SceneIsLoaded = new NetworkVariable<bool>(true);
    public float MaximumRounds;
    public float MaximumTime;
    int NumberOfRounds;
    public float Counter;
    public TextMeshProUGUI TimeContainer;
    public TextMeshProUGUI ContinueText;


    public float MovementSpeed = 32;
    public float TagSpeed = 40;
    public GameObject BlueLight;
    public GameObject RedLight;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject RedMarker;
    public GameObject BlueMarker;
    public GameObject PlayerOneCheck;
    public GameObject PlayerTwoCheck;

    public Canvas FinalCanvas;

    BoxCollider2D GroundCheckOne;
    BoxCollider2D GroundCheckTwo;
    public NetworkVariable<bool> enableFinal = new NetworkVariable<bool>();

    public TextMeshProUGUI RedScoreFContainer;
    public TextMeshProUGUI BlueScoreFContainer;
    public TextMeshProUGUI DeclareWinner;
    public NetworkVariable<bool> tagged = new NetworkVariable<bool>(false);


    private void Awake()
    {
/*        Resume();
*/  }

    private void Start()
    {
        tagged.Settings.WritePermission = NetworkVariablePermission.Everyone;
        tagged.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        CurrentTime.Settings.WritePermission = NetworkVariablePermission.ServerOnly;
        CurrentTime.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        enableFinal.Settings.WritePermission = NetworkVariablePermission.Everyone;
        enableFinal.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        SceneIsLoaded.Settings.WritePermission = NetworkVariablePermission.Everyone;
        SceneIsLoaded.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        BlueScore.Settings.WritePermission = NetworkVariablePermission.Everyone;
        BlueScore.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        RedScore.Settings.WritePermission = NetworkVariablePermission.Everyone;
        RedScore.Settings.ReadPermission = NetworkVariablePermission.Everyone;

        if (IsHost)
        {
            SpawnHostServerRpc(NetworkManager.Singleton.LocalClientId);
            RoundCompleteCanvas.enabled = false;
            FinalCanvas.enabled = false;
            SceneIsLoaded.Value = true;
            MaximumRounds = FindObjectOfType<Values>().MaximumRounds;
            MaximumTime = FindObjectOfType<Values>().MaximumTime;
            RedScore.Value = FindObjectOfType<Values>().RedScore;
            BlueScore.Value = FindObjectOfType<Values>().BlueScore;

            CurrentTime.Value = MaximumTime;
            enableFinal.Value = false;

            FindObjectOfType<AudioManager>().Play("BackGround");
            Counter = MaximumTime;
            NumberOfRounds = FindObjectOfType<Values>().currentRound;
            // setTimeServerRPC(MaximumTime);
        }
        else if (IsClient)
        {
            ContinueText.text = "Waiting for Host to Continue";
            ContinueText.fontSize = 24;
            enableFinal.Value = false;
            SceneIsLoaded.Value = true;
            RoundCompleteCanvas.enabled = false;
            FinalCanvas.enabled = false;
            Debug.Log("client");
            SpawnClientServerRpc(NetworkManager.Singleton.LocalClientId);
            FindObjectOfType<AudioManager>().Play("BackGround");
        }
    }

    // [ServerRpc]
    // public void setTimeServerRPC(float time)
    // {
    //     CurrentTime.Value = time;
    // }

/*    public void Resume()
    {
        Time.timeScale = 1f;
        pausedMenu.SetActive(false);
    }*/

/*    public void Pause()
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0f;
    }*/

    public void Restart()
    {
        SceneManager.LoadScene("Loading");
    }

    public void MainMenu()
    {
        if (IsClient)
            DestroyClientServerRpc();
        if (IsHost)
            NetworkSceneManager.SwitchScene("Hosting");
        else
            SceneManager.LoadScene("MainMenu");
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnHostServerRpc(ulong clientId)
    {
        /*        float x = -300 + 280 * ((clientId > 0) ? (int)clientId - 1 : 0);
        */
        Debug.Log("Spawning Host");
/*        float x = -100;
*/      _host = Instantiate(HostPrefab, new Vector3(25, 8, 0), Quaternion.identity).gameObject;
        _host.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
        // FindObjectOfType<PlayerOne>().SetMarker(false);

        // _host.GetComponent<NetworkObject>()
        // BlueMarkerDeactivateClientRpc();
/*        Debug.Log("host"+_host);
        Debug.Log("hosted" + _host.transform.GetChild(1).gameObject);*/
        // BlueMarker = _host.transform.GetChild(1).gameObject;
        // BlueMarker.SetActive(false);
        // ToggleBlueMarkerFalse();
/*        BlueLight = _host.transform.GetChild(3).gameObject;
*//*        BlueLight.SetActive(false);
*//*        RpcSetInactiveBlueLightServerRpc(clientId);
*//*        GroundCheckOne = PlayerOneCheck.GetComponent<BoxCollider2D>();
*/
    }

    [ClientRpc]
    private void BlueMarkerDeactivateClientRpc()
    {
        // FindObjectOfType<PlayerOne>().SetMarker(false);
        // _host.SetActive(false);
        // _host.transform.GetChild(1).gameObject.SetActive(false);
        // _host.transform.GetChild(1).gameObject.SetActive(false);
    }

/*    [ServerRpc(RequireOwnership = false)]
    void RpcSetInactiveBlueLightServerRpc(ulong clientId)
    {
        *//*        _host.transform.GetChild(3).gameObject.SetActive(false);
        *//*
        BlueLight.SetActive(false);
    }*/

/*    [ClientRpc]
    void RpcSetInactiveBlueLightClientRpc(ulong clientId)
    {
        *//*        _host.transform.GetChild(3).gameObject.SetActive(false);
        *//*
        BlueLight.SetActive(false);
    }*/


    [ServerRpc(RequireOwnership = false)]
    private void SpawnClientServerRpc(ulong clientId)
    {
        Debug.Log("Spawning Client");
        /*        float x = -300 + 280 * ((clientId > 0) ? (int)clientId - 1 : 0);
        */
/*        float x = 100;
*/      _client = Instantiate(ClientPrefab, new Vector3(29, 8, 0), Quaternion.identity).gameObject;
        _client.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);

/*        GroundCheckTwo = PlayerTwoCheck.GetComponent<BoxCollider2D>();
*/
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

    public void Continue()
    {
//         if(FindObjectOfType<Values>().currentRound > FindObjectOfType<Values>().MaximumRounds)
//         {
//             FindObjectOfType<Values>().LoadScene(6);
//         }
//         else
//         {
//             FindObjectOfType<Values>().LoadScene(Random.Range(1, 6));
//         }

// //        RoundCompleteCanvas.enabled = false;
//         FindObjectOfType<AudioManager>().Stop("BackGround2");
//         FindObjectOfType<AudioManager>().Play("BackGround");
//         FindObjectOfType<GroundCheck1>().GrounCheck1 = true;
//         FindObjectOfType<GroundCheck2>().GrounCheck2 = true;
//         FindObjectOfType<Timer>().SceneIsLoaded = true;
        NetworkSceneManager.SwitchScene(nextLevel);

    }

    public void Menu()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkSceneManager.SwitchScene("Lobby");
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkSceneManager.SwitchScene("Lobby");
            NetworkManager.Singleton.StopClient();
        }
    }



    public void NewGame()
    {
        FindObjectOfType<AudioManager>().Stop("BackGround2");
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
        FindObjectOfType<Values>().RedScore = 0;
        FindObjectOfType<Values>().BlueScore = 0;
        FindObjectOfType<Values>().MaximumRounds = 0;
        SceneManager.LoadScene(0);

    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Stop("BackGround2");
        Application.Quit();
    }

    void Update()
    {
/*        Physics2D.IgnoreCollision(_host.GetComponent<Collider2D>(), _client.GetComponent<Collider2D>());
*/
        RedScoreContainer.text = RedScore.Value.ToString();
        BlueScoreContainer.text = BlueScore.Value.ToString();
        FindObjectOfType<Values>().RedScore = RedScore.Value;
        FindObjectOfType<Values>().BlueScore = BlueScore.Value;

        if(enableFinal.Value)
        {
            Debug.Log("yeah");
            FindObjectOfType<AudioManager>().Play("BackGround2");
            RedScoreFContainer.text = RedScore.Value.ToString();
            BlueScoreFContainer.text = BlueScore.Value.ToString();


            if (BlueScore.Value > RedScore.Value)
            {
                DeclareWinner.text = "Blue Wins!";
            }
            if (BlueScore.Value  < RedScore.Value)
            {
                DeclareWinner.text = "Red Wins!";
            }
            if (BlueScore.Value  == RedScore.Value)
            {
                DeclareWinner.text = "Tie!";
            }
            FinalCanvas.enabled = true;
        }

        if (IsHost)
        {

// BlueMarkerDeactivateClientRpc();
            // Debug.Log("YeahHost" + FindObjectOfType<NetworkVar>().CurrentTime.Value);
            if (SceneIsLoaded.Value == false)
            {
                /*          FindObjectOfType<RoundComplete>().RedScore.text = RedScore.ToString();
                            FindObjectOfType<RoundComplete>().BlueScore.text = BlueScore.ToString();
                            Debug.Log("Red"+FindObjectOfType<RoundComplete>().RedScore.text);
                            Debug.Log("Blue"+FindObjectOfType<RoundComplete>().BlueScore.text);*/
                FindObjectOfType<AudioManager>().Stop("BackGround");
                RoundCompleteCanvas.enabled = true;
                Counter = MaximumTime;
            }
            else
            {
                // TimeContainer.text = CurrentTime.Value.ToString();
                TimeContainer.text = CurrentTime.Value.ToString();
                Counter = Counter - 1f*Time.deltaTime;
                CurrentTime.Value = Mathf.RoundToInt(Counter);

                if (Counter <= 0f)
                {
                    FindObjectOfType<Values>().currentRound = FindObjectOfType<Values>().currentRound + 1;
                    FindObjectOfType<AudioManager>().Play("BackGround2");
                    /*if (CollisonCheck.GetComponent<CollisonScript>().Turn)
                    {
                        FindObjectOfType<Values>().RedScore = FindObjectOfType<Values>().RedScore + 1;
                        *//*                    FindObjectOfType<RoundComplete>().RedScore.text = RedScore.ToString();
                        *//*
                        Counter = MaximumTime;
                    }
                    if (!CollisonCheck.GetComponent<CollisonScript>().Turn)
                    {
                        FindObjectOfType<Values>().BlueScore = FindObjectOfType<Values>().BlueScore + 1;
                        *//*                    FindObjectOfType<RoundComplete>().BlueScore.text = BlueScore.ToString();
                        *//*
                        Counter = MaximumTime;
                    }*/
                    if(!FindObjectOfType<PlayerOne>().BlueMarker.activeSelf)
                    {
                        FindObjectOfType<Values>().BlueScore = FindObjectOfType<Values>().BlueScore + 1;
                        BlueScore.Value = BlueScore.Value + 1;
                    }
                    else
                    {
                        FindObjectOfType<Values>().RedScore = FindObjectOfType<Values>().RedScore + 1;
                        RedScore.Value = RedScore.Value + 1;
                    }

                    if (FindObjectOfType<Values>().currentRound > MaximumRounds)
                    {    
                        FindObjectOfType<Values>().RedScore = RedScore.Value;
                        FindObjectOfType<Values>().BlueScore = BlueScore.Value;
                        enableFinal.Value = true;
                        // NetworkSceneManager.SwitchScene("NetworkDeclare");
                    }

                        SceneIsLoaded.Value = false;
                }
            }    
            if (FindObjectOfType<Values>().currentRound > MaximumRounds)
                    {    
                        FindObjectOfType<Values>().RedScore = RedScore.Value;
                        FindObjectOfType<Values>().BlueScore = BlueScore.Value;
                        enableFinal.Value = true;
                        // NetworkSceneManager.SwitchScene("NetworkDeclare");
                    }        
            /*        FindObjectOfType<Values>().RedScore = RedScore;
                    FindObjectOfType<Values>().BlueScore = BlueScore;*/

            // if (FindObjectOfType<Values>().currentRound > MaximumRounds)
            // {/*
            // FindObjectOfType<Values>().RedScore = RedScore;
            // FindObjectOfType<Values>().BlueScore = BlueScore;*/
            //     // NetworkSceneManager.SwitchScene("NetworkDeclare");
            // }
        }
        else if (IsClient)
        {
            if (SceneIsLoaded.Value == false)
            {
                    /*          FindObjectOfType<RoundComplete>().RedScore.text = RedScore.ToString();
                                FindObjectOfType<RoundComplete>().BlueScore.text = BlueScore.ToString();
                                Debug.Log("Red"+FindObjectOfType<RoundComplete>().RedScore.text);
                                Debug.Log("Blue"+FindObjectOfType<RoundComplete>().BlueScore.text);*/
                FindObjectOfType<AudioManager>().Stop("BackGround");
                RoundCompleteCanvas.enabled = true;
                Counter = MaximumTime;
            }
            else
            {
                TimeContainer.text = CurrentTime.Value.ToString();
            }
            // Counter = Counter - 1f * Time.deltaTime;
        }
    }
}

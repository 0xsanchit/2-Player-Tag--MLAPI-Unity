using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using TMPro;
using MLAPI.NetworkVariable;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;



public class NetworkRetry : NetworkBehaviour
{
    public TextMeshProUGUI RedScoreContainer;
    public TextMeshProUGUI BlueScoreContainer;
    public TextMeshProUGUI DeclareWinner;
    public NetworkVariable<float> BlueScore = new NetworkVariable<float>();
    public NetworkVariable<float> RedScore = new NetworkVariable<float>();

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BackGround2");
        BlueScore.Settings.WritePermission = NetworkVariablePermission.Everyone;
        BlueScore.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        RedScore.Settings.WritePermission = NetworkVariablePermission.Everyone;
        RedScore.Settings.ReadPermission = NetworkVariablePermission.Everyone;
         if(IsHost)
        {
            BlueScore.Value = FindObjectOfType<Values>().BlueScore;
            RedScore.Value = FindObjectOfType<Values>().RedScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsHost)
        {
            Debug.Log("Updating Values");
            BlueScore.Value = FindObjectOfType<Values>().BlueScore;
            RedScore.Value = FindObjectOfType<Values>().RedScore;
        }
        RedScore.Value = FindObjectOfType<Values>().RedScore;
        BlueScore.Value = FindObjectOfType<Values>().BlueScore;
        RedScoreContainer.text = RedScore.Value.ToString();
        BlueScoreContainer.text = BlueScore.Value.ToString();


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
}

using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Values : MonoBehaviour
{
    public float RedScore;
    public float BlueScore;
    public float MaximumRounds;
    public float MaximumTime;
    public float Volume;
    public int numPlayers;
    public int currentRound;
    
    public static Values instance;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    

    public void LoadScene(int Number)
    {
        DontDestroyOnLoad(gameObject);

        FindObjectOfType<LoadLevel>().LoadGameLevel(Number);
    }
}

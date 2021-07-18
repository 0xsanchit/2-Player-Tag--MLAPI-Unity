using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using MLAPI;

public class TimerNetwork : NetworkBehaviour
{
    
    public bool SceneIsLoaded;
    public float MaximumRounds;
    public float MaximumTime;
    int NumberOfRounds;
    public Canvas RoundCompleteCanvas;
    public GameObject CollisonCheck;
    public float Counter;
    public float BlueScore;
    public float RedScore;
    float CurrentTime;
    public TextMeshProUGUI TimeContainer;
    

    private void Start()
    {
        RoundCompleteCanvas.enabled = false;
        SceneIsLoaded = true;
        MaximumRounds = FindObjectOfType<Values>().MaximumRounds;
        MaximumTime = FindObjectOfType<Values>().MaximumTime;
        RedScore = FindObjectOfType<Values>().RedScore;
        BlueScore = FindObjectOfType<Values>().BlueScore;
        FindObjectOfType<AudioManager>().Play("BackGround");
        Counter = MaximumTime;
        NumberOfRounds = FindObjectOfType<Values>().currentRound;
    }
    private void Update()
    {
        if (SceneIsLoaded == false)
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
            TimeContainer.text = CurrentTime.ToString();
            Counter = Counter - 1f * Time.deltaTime;
            CurrentTime = Mathf.RoundToInt(Counter);

            if (Counter <= 0f)
            {
                FindObjectOfType<Values>().currentRound = FindObjectOfType<Values>().currentRound + 1;
                FindObjectOfType<AudioManager>().Play("BackGround2");
                if (CollisonCheck.GetComponent<CollisonScript>().Turn)
                {
                    FindObjectOfType<Values>().RedScore = FindObjectOfType<Values>().RedScore + 1;
/*                    FindObjectOfType<RoundComplete>().RedScore.text = RedScore.ToString();
*/                    Counter = MaximumTime;
                }
                if (!CollisonCheck.GetComponent<CollisonScript>().Turn)
                {
                    FindObjectOfType<Values>().BlueScore = FindObjectOfType<Values>().BlueScore + 1;
/*                    FindObjectOfType<RoundComplete>().BlueScore.text = BlueScore.ToString();
*/                    Counter = MaximumTime;
                }
                if (NumberOfRounds == MaximumRounds)
                {/*
                    FindObjectOfType<Values>().RedScore = RedScore;
                    FindObjectOfType<Values>().BlueScore = BlueScore;*/
                    SceneManager.LoadScene(6);
                }
                SceneIsLoaded = false;
            }
        }
/*        FindObjectOfType<Values>().RedScore = RedScore;
        FindObjectOfType<Values>().BlueScore = BlueScore;*/

        if (NumberOfRounds > MaximumRounds)
        {/*
            FindObjectOfType<Values>().RedScore = RedScore;
            FindObjectOfType<Values>().BlueScore = BlueScore;*/
            SceneManager.LoadScene(6);
        }        
    }
}

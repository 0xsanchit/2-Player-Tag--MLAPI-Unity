using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuScriptNetwork : MonoBehaviour
{
    
    public float MaximumRounds;
    public float MaximumTime;
    public TextMeshProUGUI TimeSelectedText;
    public TextMeshProUGUI RoundsSelectedText;
    
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BackGround");
        MaximumTime = 10;
        MaximumRounds = 1;
    }
    
    void Update()
    {
        TimeSelectedText.text = MaximumTime.ToString() + "  Sec";
        RoundsSelectedText.text = MaximumRounds.ToString();
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
        if (MaximumRounds >=2)
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
        if(FindObjectOfType<Values>().numPlayers == 1)
        {
            SceneManager.LoadScene(Random.Range(1,6));
        }
        else
        {
            FindObjectOfType<Values>().currentRound = 1;
            SceneManager.LoadScene(Random.Range(1, 6));
        }
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
    
    public void Menu()
    {

    }
    
}
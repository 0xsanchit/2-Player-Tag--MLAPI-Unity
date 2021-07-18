using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RetryScript : MonoBehaviour
{
    public TextMeshProUGUI RedScore;
    public TextMeshProUGUI BlueScore;
    public TextMeshProUGUI DeclareWinner;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BackGround2");

        if(FindObjectOfType<Values>().numPlayers==1)
        {
            if (FindObjectOfType<Values>().BlueScore > FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "AI Dominates the World";
            }
            if (FindObjectOfType<Values>().BlueScore < FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "You win !";
            }
            if (FindObjectOfType<Values>().BlueScore == FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "Tie!";
            }
        }
        else if (FindObjectOfType<Values>().numPlayers == 2)
        {
            if (FindObjectOfType<Values>().BlueScore > FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "Blue Wins!";
            }
            if (FindObjectOfType<Values>().BlueScore < FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "Red Wins!";
            }
            if (FindObjectOfType<Values>().BlueScore == FindObjectOfType<Values>().RedScore)
            {
                DeclareWinner.text = "Tie!";
            }
        }




        RedScore.text = FindObjectOfType<Values>().RedScore.ToString();
        BlueScore.text = FindObjectOfType<Values>().BlueScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        FindObjectOfType<AudioManager>().Stop("BackGround2");
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

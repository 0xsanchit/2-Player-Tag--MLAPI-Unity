using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using MLAPI;

public class RoundCompleteNetwork : NetworkBehaviour
{
   
    public Canvas RoundCompleteCanvas;
    public TextMeshProUGUI RedScore;
    public TextMeshProUGUI BlueScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RedScore.text = FindObjectOfType<Values>().RedScore.ToString();
        BlueScore.text = FindObjectOfType<Values>().BlueScore.ToString();
    }

    public void Continue()
    {
        if(FindObjectOfType<Values>().currentRound >= FindObjectOfType<Values>().MaximumRounds)
        {
            FindObjectOfType<Values>().LoadScene(6);
        }
        else
        {
            FindObjectOfType<Values>().LoadScene(Random.Range(1, 6));
        }

//        RoundCompleteCanvas.enabled = false;
        FindObjectOfType<AudioManager>().Stop("BackGround2");
        FindObjectOfType<AudioManager>().Play("BackGround");
        FindObjectOfType<GroundCheck1>().GrounCheck1 = true;
        FindObjectOfType<GroundCheck2>().GrounCheck2 = true;

        FindObjectOfType<Timer>().SceneIsLoaded = true;
    }

/*    public void SingleContinue()
    {
        FindObjectOfType<Values>().LoadScene(Random.Range(2,3));
        RoundCompleteCanvas.enabled = false;
        FindObjectOfType<AudioManager>().Stop("BackGround2");
        FindObjectOfType<AudioManager>().Play("BackGround");
*//*        FindObjectOfType<GroundCheck1>().GrounCheck1 = true;
*//*        FindObjectOfType<GroundCheck2>().GrounCheck2 = true;

        FindObjectOfType<SingleTimer>().SceneIsLoaded = true;

    }*/
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        FindObjectOfType<Values>().LoadScene(0);
        FindObjectOfType<Values>().RedScore = 0;
        FindObjectOfType<Values>().BlueScore = 0;
    }

}

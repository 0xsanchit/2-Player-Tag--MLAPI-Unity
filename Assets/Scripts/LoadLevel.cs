using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public Animator Transition;
    public void LoadGameLevel(int SceneIndex)
    {
        StartCoroutine(LoadScene(SceneIndex));
    }
    IEnumerator LoadScene(int Scenenumber)
    {
        Transition.SetTrigger("Play Transition");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(Scenenumber);
    }
}

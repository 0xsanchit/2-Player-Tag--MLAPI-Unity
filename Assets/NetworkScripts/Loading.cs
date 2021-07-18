using System.Collections;
using MLAPI;
using MLAPI.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : NetworkBehaviour
{
	public Slider bar;
	
	
	void Start()
	{
		if (IsServer)
		{
			Debug.Log("Loading Scene Server");
			NetworkSceneManager.SwitchScene("LevelOne");
		}
		else
		{
			if (IsClient)
				StartCoroutine(LoadYourAsyncScene());
		}
	}
	
	IEnumerator  LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelOne");
        
		while (asyncLoad.progress < 1)
		{
			Debug.Log("Loading Scene Client");
			bar.value = asyncLoad.progress;
			yield return new WaitForEndOfFrame();
		}
	}
	
}
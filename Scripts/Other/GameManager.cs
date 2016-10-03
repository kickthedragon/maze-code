using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static bool isRetrying;

//	static GameManager instance;
//
//	void Awake()
//	{
//		if (instance == null) {
//			DontDestroyOnLoad (gameObject);
//			instance = this;
//		} else
//			Destroy (gameObject);
//	}
//
//	void OnDestroy()
//	{
//		if (instance == this)
//			instance = null;
//	}


	public static void QuitGame()
	{
		Application.Quit ();
	}

	public static void ResumeGame()
	{
		PlayerEventManager.FirePlayerCloseMenu ();
	}

	public static void RestartGame()
	{
		isRetrying = true;
		Application.LoadLevel (0);
		TimeManager.ResetTimer ();
	}

	public void Quit()
	{
		QuitGame ();
	}
	
	public void Resume()
	{
		ResumeGame ();
	}
	
	public void Restart()
	{
		RestartGame ();
	}

	public void OpenMenu()
	{
		PlayerEventManager.FirePlayerOpenMenu ();
	}

	public void CloseMenu()
	{
		PlayerEventManager.FirePlayerCloseMenu ();
	}
}

using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {

	public GameObject optionsWindow;

	public GameObject touchHandler;

	void OnEnable()
	{
		PlayerEventManager.OnPlayerOpenMenu += OpenOptionsMenu;
		PlayerEventManager.OnPlayerCloseMenu += CloseOptionsMenu;
	}

	void OnDisable()
	{
		PlayerEventManager.OnPlayerOpenMenu -= OpenOptionsMenu;
		PlayerEventManager.OnPlayerCloseMenu -= CloseOptionsMenu;
	}

	void OpenOptionsMenu()
	{
		optionsWindow.SetActive (true);
		touchHandler.SetActive (false);
	}

	void CloseOptionsMenu()
	{
		optionsWindow.SetActive (false);
		touchHandler.SetActive (true);
	}
}

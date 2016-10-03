using UnityEngine;
using System.Collections;

public class IPlayerInput : MonoBehaviour {
	
	protected PlayerEventManager eventManager;
	
	protected virtual void Awake()
	{
		eventManager = GetComponent<PlayerEventManager>();
	}
	


	protected void PlayerMove(Vector3 vect) { PlayerEventManager.FirePlayerMove (vect);}

	protected void PlayerMenu(bool allowingInput) { 
		if (allowingInput) 
			PlayerEventManager.FirePlayerOpenMenu ();
		else
			PlayerEventManager.FirePlayerCloseMenu ();
		}

	protected void ToggleDebug() { PlayerEventManager.FirePlayerToggleDebug (); }

	protected void ToggleTimer() { PlayerEventManager.FirePlayerToggleTimer (); }

	protected void ZoomIn() {PlayerEventManager.FirePlayerZoomIn ();}

	protected void ZoomOut() {PlayerEventManager.FirePlayerZoomOut ();}
}
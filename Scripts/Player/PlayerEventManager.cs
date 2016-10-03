using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Player Event Manager
/// 
/// Currently using different delegate signatures because UnityNetworking doesns't support multiple sync events signed to a delegate for unity networkng support
/// This will be re-written to support unity networking soon to support unity networking.
/// </summary>

public class PlayerEventManager : MonoBehaviour {
	
	#region MOVEMENT EVENTS
	

	public static event Action<Vector3> OnPlayerMove;
	public static void FirePlayerMove(Vector3 vect) { if (OnPlayerMove != null) OnPlayerMove(vect); }

	public static event Action OnPlayerZoomIn;
	public static void FirePlayerZoomIn() { if (OnPlayerZoomIn != null) OnPlayerZoomIn(); }

	public static event Action OnPlayerZoomOut;
	public static void FirePlayerZoomOut() { if (OnPlayerZoomOut != null) OnPlayerZoomOut(); }

	public static event Action OnPlayerMadeFirstMove;
	public static void FirePlayerMadeFirstMove () { if (OnPlayerMadeFirstMove != null) OnPlayerMadeFirstMove(); }

	public static event Action<float,float> OnPlayerSpawned;
	public static void FirePlayerSpawned(float x, float z) { if (OnPlayerSpawned != null) OnPlayerSpawned(x,z); }

	public static event Action<float,float> OnPlayerUpdatePosition;
	public static void FireUpdatePosition(float x, float z) { if (OnPlayerUpdatePosition != null) OnPlayerUpdatePosition(x,z); }

	#endregion
	

	#region UI EVENTS
	

	public static event Action OnPlayerOpenMenu;
	public static void FirePlayerOpenMenu() { if (OnPlayerOpenMenu != null) OnPlayerOpenMenu(); }

	public static event Action OnPlayerCloseMenu;
	public static void FirePlayerCloseMenu() { if (OnPlayerCloseMenu != null) OnPlayerCloseMenu(); }

	public static event Action OnPlayerToggleDebug;
	public static void FirePlayerToggleDebug() { if (OnPlayerToggleDebug != null) OnPlayerToggleDebug(); }

	public static event Action OnPlayerToggleTimer;
	public static void FirePlayerToggleTimer() { if (OnPlayerToggleTimer != null) OnPlayerToggleTimer(); }


	#endregion
}
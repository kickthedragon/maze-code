using UnityEngine;
using InControl;
using System;
using System.Collections;

public class RumbleControl : MonoBehaviour {

	public static event Action OnRumble;
	public static void FireRumble() { if (OnRumble != null) OnRumble(); }

	public static bool isRumbling {	get; private set; }

	public bool rumbleEnabled;

	public static bool rumbleSupported { get { return (Application.isMobilePlatform || InputManager.ActiveDevice.Name.Contains("Xbox")) ? true:false; } }

	public float rumbleTime = 1f;



	void OnEnable()
	{
		OnRumble += rumble;
	}

	void OnDisable()
	{
		OnRumble -= rumble;
	}

	void rumble()
	{
		if (isRumbling)
			return;
	
	#if !UNITY_STANDALONE_OSX
		if(rumbleEnabled && rumbleSupported)
			StartCoroutine (rumbleRoutine ());
#endif
	}

#if UNITY_ANDROID || UNITY_IPHONE
	IEnumerator rumbleRoutine()
	{
		isRumbling = true;

		Handheld.Vibrate ();
		yield return new WaitForSeconds(1f);
		Handheld.Vibrate ();

		isRumbling = false;

		yield break;

	}
#endif

#if UNITY_STANDALONE
	IEnumerator rumbleRoutine()
	{
		isRumbling = true;
		
		InputManager.ActiveDevice.Vibrate (.6f);
		yield return new WaitForSeconds(1f);
        InputManager.ActiveDevice.StopVibration();

        isRumbling = false;

        yield break;
		
	}
#endif

	public void ToggleRumble()
	{
		rumbleEnabled = !rumbleEnabled;
	}

	public void ToggleRumble(bool state)
	{
		rumbleEnabled = state;
	}
}

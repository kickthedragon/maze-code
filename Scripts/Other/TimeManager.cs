using UnityEngine;
using System;
using System.Collections;


public class TimeManager : MonoBehaviour {

	public static TimeManager Instance { get; private set; }

	public static float playerTime { get; private set; }


	public static string PlayerTimeCurrent { get { return string.Format("{0}:{1:00}", Mathf.FloorToInt(playerTime / 60), Mathf.FloorToInt(playerTime % 60)); } }

	public static event Action<float> UpdateTime;

	static void FireUpdateTime(float time) {
		if (UpdateTime != null)
			UpdateTime (time);
	}

	void OnEnable()
	{
		PlayerEventManager.OnPlayerMadeFirstMove += initialStart;
		PlayerEventManager.OnPlayerOpenMenu += PauseTimer;
		PlayerEventManager.OnPlayerCloseMenu += StartTimer;
	}

	void OnDisable()
	{
		PlayerEventManager.OnPlayerMadeFirstMove -= initialStart;
		PlayerEventManager.OnPlayerOpenMenu -= PauseTimer;
		PlayerEventManager.OnPlayerCloseMenu -= StartTimer;
	}


	void Awake()
	{
		if (Instance == null) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			DontDestroyOnLoad(gameObject);
			init();
			Instance = this;
		}
		else
			Destroy (gameObject);
	}

	void OnDestroy()
	{
		if (Instance == this) {
			Instance = null;
		}
	}

	void init()
	{
		playerTime = Settings.GetPlayerTime ();
		FireUpdateTime (playerTime);
	}

	void initialStart()
	{

		StartTimer ();
	}

	void StartTimer()
	{
		StartCoroutine ("timer");
	}


	IEnumerator timer()
	{
		while (true) {

			playerTime += Time.deltaTime;
			Settings.SetPlayerTime(playerTime);
			FireUpdateTime(playerTime);
			yield return null;
		}
	}

	void PauseTimer()
	{
		StopCoroutine ("timer");
	}

	public static void ResetTimer()
	{
		playerTime = 0;
	}
}

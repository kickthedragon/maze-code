using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITimer : MonoBehaviour {

	static UITimer instance;

	Text timerText;

	static bool showing;

	static bool fading;

	public float fadeSpeed = .5f;

	void Awake()
	{
		if (instance == null) {
			timerText = GetComponent<Text> ();
			instance = this;
		} else
			Destroy (this);

	}

	void OnDestroy() 
	{
		if (instance == this)
			instance = null;
	}

	void OnEnable()
	{
		TimeManager.UpdateTime += updateText;
		PlayerEventManager.OnPlayerToggleTimer += ToggleTimer;
	}

	void OnDisable()
	{
		TimeManager.UpdateTime -= updateText;
		PlayerEventManager.OnPlayerToggleTimer -= ToggleTimer;
	}

	void updateText(float time)
	{
		timerText.text = string.Format("{0}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time % 60));
	}

	void ToggleTimer()
	{
		if (fading)
			return;

		if (showing)
			HideTimer ();
		else
			ShowTimer ();
	}

	public static void ShowTimer()
	{

		fading = true;
		instance.StartCoroutine (instance.fadeIn (instance.fadeSpeed));
	}

	public static void HideTimer()
	{

		fading = true;
		instance.StartCoroutine (instance.fadeOut (instance.fadeSpeed));
	}

	IEnumerator fadeOut (float fadeOutTime)
	{
				
		float timer = 0;
		do {
			
			timerText.color= Color.Lerp(timerText.color, new Color(timerText.color.r, timerText.color.g, timerText.color.b, 0), timer/fadeOutTime);
			
			timer += Time.deltaTime;
			yield return null;
		}while(timer < fadeOutTime);

		showing = false;
		fading = false;
	}

	IEnumerator fadeIn (float fadeInTime)
	{
		
		float timer = 0;
		do {
			
			timerText.color= Color.Lerp(timerText.color, new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1f), timer/fadeInTime);
			
			timer += Time.deltaTime;
			yield return null;
		}while(timer < fadeInTime);

		showing = true;
		fading = false;
	}
}

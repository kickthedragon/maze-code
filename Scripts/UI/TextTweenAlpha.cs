using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTweenAlpha : MonoBehaviour {


	public float fadeOutTime;

	public float fadeInTime;

	public float delay;

	private Text text;

	public bool fadeOnGenerated;

	public bool fadeIn;

	public bool fadeOut;


	void Awake()
	{
		text = GetComponent<Text> ();
	}

	void Start()
	{
		if (!fadeOnGenerated)
			StartFade ();
		
	}

	void OnEnable()
	{
		if(fadeOnGenerated)
			MazeGenerator.OnGenerated += StartFade;
	}

	void OnDisable()
	{
		if(fadeOnGenerated)
			MazeGenerator.OnGenerated -= StartFade;
	}

	void StartFade()
	{
		if (fadeOut)
			StartCoroutine (fadeOutRoutine (fadeOutTime));
		else if (fadeIn)
			StartCoroutine (fadeInRoutine (fadeInTime));


	}

	IEnumerator fadeOutRoutine (float fadeOutTime)
	{
		yield return new WaitForSeconds(delay);

		float timer = 0;
		do {

			text.color= Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), timer/fadeOutTime);
		
			timer += Time.deltaTime;
			yield return null;
		}while(timer < fadeOutTime);

	}

	IEnumerator fadeInRoutine (float fadeIn)
	{
		yield return new WaitForSeconds(delay);
		
		float timer = 0;
		do {
			
			text.color= Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 1f), timer/fadeIn);
			
			timer += Time.deltaTime;
			yield return null;
		}while(timer < fadeIn);
		
	}
}

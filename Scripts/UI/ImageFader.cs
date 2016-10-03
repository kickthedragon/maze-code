using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFader : MonoBehaviour {

	private Image fader;


	public float fadeOutTime;
	
	public float delay;

	
	void Awake()
	{
		fader = GetComponent<Image> ();
	}

	
	void OnEnable()
	{
		MazeGenerator.OnGenerated += StartFade;
	}
	
	void OnDisable()
	{
		MazeGenerator.OnGenerated -= StartFade;
	}
	
	void StartFade()
	{
		StartCoroutine (fadeOut (fadeOutTime));
		
	}
	
	IEnumerator fadeOut (float fadeOutTime)
	{
		yield return new WaitForSeconds(delay);
		
		float timer = 0;

		float timer2 = 0;

		if (fader.color != Color.white) {
		
			do
			{
				fader.color = Color.Lerp(fader.color, Color.white, timer2/1f);
				timer2 += Time.deltaTime;
				yield return null;
				                       
			}while(timer2<1f);
		}

		do {

			fader.color= Color.Lerp(fader.color, new Color(0, 0, 0, 0), timer/fadeOutTime);
			
			timer += Time.deltaTime;
			yield return null;
		}while(timer < fadeOutTime);
		
	}
}

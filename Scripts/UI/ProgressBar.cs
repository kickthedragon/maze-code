using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	Slider slider;

	void Awake()
	{
		slider = GetComponent<Slider> ();
	}

	void OnEnable()
	{
		MazeGenerator.OnUpdateGenerateProgress += updateBar;
	}

	void OnDisable()
	{
		MazeGenerator.OnUpdateGenerateProgress -= updateBar;
	}

	void updateBar(float progress)
	{
		slider.value = progress;
	}
}

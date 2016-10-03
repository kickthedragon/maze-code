using UnityEngine;
using System.Collections;

public class DisableOnGenerated : MonoBehaviour {

	void OnEnable()
	{
		MazeGenerator.OnGenerated += Disable;
	}
	void OnDisable()
	{
		MazeGenerator.OnGenerated -= Disable;
	}

	void Disable()
	{
		gameObject.SetActive (false);
	}
}

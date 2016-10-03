using UnityEngine;
using System.Collections;

public class DebugCameraControls : MonoBehaviour {

	public float speedFactor = 1.5f;



	void OnEnable()
	{
		PlayerEventManager.OnPlayerZoomIn += ZoomIn;
		PlayerEventManager.OnPlayerZoomOut += ZoomOut;
	}

	void OnDisable()
	{
		PlayerEventManager.OnPlayerZoomIn -= ZoomIn;
		PlayerEventManager.OnPlayerZoomOut -= ZoomOut;
	}




	void ZoomIn()
	{
		SmoothFollow.zoomMin -= Time.deltaTime * speedFactor;
	}

	void ZoomOut()
	{
		SmoothFollow.zoomMin += Time.deltaTime * speedFactor;
	}
}

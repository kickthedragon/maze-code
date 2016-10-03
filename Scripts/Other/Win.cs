using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Analytics.gua.sendEventHit("First Maze Completion Time", TimeManager.playerTime.ToString());
			AutoFade.LoadLevel("Win Scene", 3f, 3f, Color.white);
		}
	}
}

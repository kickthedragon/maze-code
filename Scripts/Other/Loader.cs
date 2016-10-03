using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AutoFade.LoadLevel ("Game Scene", 1f, 8f, Color.black);
	}
	

}

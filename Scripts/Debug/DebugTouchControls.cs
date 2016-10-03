using UnityEngine;
using System.Collections;

public class DebugTouchControls : MonoBehaviour {

	void OnPointerDown()
	{
		Debug.Log ("On Pointer Down " + gameObject.name);
	}


	void OnPointerUp()
	{
		Debug.Log ("On Pointer Up " + gameObject.name);
	}

	void OnPointerEnter()
	{
		Debug.Log ("On Pointer Enter " + gameObject.name);
	}


	void OnPointerExit()
	{
		Debug.Log ("On Pointer Exit " + gameObject.name);
	}
}

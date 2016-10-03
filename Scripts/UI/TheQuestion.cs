using UnityEngine;
using System.Collections;

public class TheQuestion : MonoBehaviour {

	public void AnswerYes ()
	{
		Yes ();
	}

	public void AnswerNo ()
	{
		No ();
	}

	public static void Yes()
	{
		Analytics.gua.sendEventHit ("was it worth it?", "Yes");
		AutoFade.LoadLevel (0, 1f, 3f, Color.white);
	}

	public static void No()
	{
		Analytics.gua.sendEventHit ("was it worth it?", "No");
		AutoFade.LoadLevel (0, 1f, 3f, Color.white);
	}
}

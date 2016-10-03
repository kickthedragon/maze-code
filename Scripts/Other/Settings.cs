using UnityEngine;
using System.Collections;

public static class Settings  {

	const string PLAYER_POSITION = "Player Position";

	const string PLAYER_TIME = "Player Time";

	const string MUSIC_VOLUME = "Music Volume";

	const string RESET_PREFS = "Reset Prefs";

	public static void SetPlayerPosition(string position)
	{
		PlayerPrefs.SetString (PLAYER_POSITION, position);
	}

	public static string GetPlayerPosition()
	{
		return PlayerPrefs.GetString (PLAYER_POSITION, string.Empty);
	}

	public static void SetMusicVolume(float volume)
	{
		PlayerPrefs.SetFloat (MUSIC_VOLUME, volume);
	}
	
	public static float GetMusicVolume()
	{
		return PlayerPrefs.GetFloat (MUSIC_VOLUME, .65f);
	}

	public static void SetPlayerTime(float time)
	{
		PlayerPrefs.SetFloat (PLAYER_TIME, time);
	}
	
	public static float GetPlayerTime()
	{
		return PlayerPrefs.GetFloat (PLAYER_TIME, 0);
	}

	public static void ResetPosition()
	{
		PlayerPrefs.DeleteKey (PLAYER_TIME);
		PlayerPrefs.DeleteKey (PLAYER_POSITION);
	}

	public static void SetResetPrefs (int state)
	{
		PlayerPrefs.SetInt (RESET_PREFS, state);
	}

	public static int GetResetPrefs ()
	{
		return PlayerPrefs.GetInt (RESET_PREFS, 1);
	}

	/// <summary>
	/// Checks for Y change. In player
	/// Make's sure player is in correct point in Y space just in case rigidbody freeze and isKenematic fails
	/// </summary>
	public static Vector3 ParseVector3String(string vect)
	{

		char[] sep = {','};
		string[] s = vect.Split(sep);
		return new Vector3(float.Parse(s[0]),float.Parse(s[1]),float.Parse(s[2]));

	}

	public static string Vector3ToString(Vector3 pos)
	{
		return pos.x + "," + pos.y + "," + pos.z;
	}

}

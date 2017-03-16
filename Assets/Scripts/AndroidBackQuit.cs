using UnityEngine;

public class AndroidBackQuit : BackButton
{
	protected override void OnBackPressed()
	{
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		#endif
	}
}
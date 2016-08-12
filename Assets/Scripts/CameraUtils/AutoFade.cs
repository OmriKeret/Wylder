using UnityEngine;
using System.Collections;
using System;

public class AutoFade : MonoBehaviour
{
	private static AutoFade m_Instance = null;
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	private SpriteRenderer blackScreen;
	private bool isFading;
	private static AutoFade Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
			}
			return m_Instance;
		}
	}
		
	private void Awake()
	{
		DontDestroyOnLoad(this);
		m_Instance = this;
		// Set the texture so that it is the the size of the screen and covers it.
		blackScreen = this.GetComponent<SpriteRenderer>();

	}
	

	private IEnumerator FadeOutCallback(Color aColor, Action callback)
	{

		while (blackScreen.color.a < 0.95f)
		{
			yield return new WaitForEndOfFrame();
			blackScreen.color = Color.Lerp(blackScreen.color, aColor, fadeSpeed * Time.deltaTime);
		}

		yield return null; // skip first frame

		isFading = false;
		if (callback != null) 
			callback ();
	}

	private IEnumerator FadeInCallback(Action callback)
	{

		while (blackScreen.color.a > 0.05f)
		{
			yield return new WaitForEndOfFrame();
			blackScreen.color = Color.Lerp(blackScreen.color, Color.clear, fadeSpeed * Time.deltaTime);
		}
		blackScreen.color = Color.clear;

		yield return null; // skip first frame
		isFading = false;
		if (callback != null) 
			callback ();
	}

	public void startFadeIn(Action callback)
	{
		if (isFading) {
			return;
		}
		StartCoroutine (FadeInCallback (callback));
	}

	public static void FadeIn(Action callback) {
		
		Instance.startFadeIn(callback);
	}

	public void startFadeOut (Color aColor, Action callback)
	{
		if (isFading) {
			return;
		}
		StartCoroutine (FadeOutCallback (aColor, callback));
	}

	public static void FadeOut(Color aColor, Action callback) {
		Instance.startFadeOut(aColor, callback);
	}
}

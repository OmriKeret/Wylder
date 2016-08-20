using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraUtils : MonoBehaviour {


	public SpriteRenderer cameraRedBlur;
	public SpriteRenderer cameraRedFilter;

	public bool blink = false;
	public bool increment = true;
	public float opac = 0.8f;

	public float blinkLen = 0.3f;
	public float regLen = 5f;
	private float baseOpacity = 0f;

	public bool regeneration;

	public static CameraUtils m_Instance = null;

	public static CameraUtils Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = (new GameObject("CameraUtils")).AddComponent<CameraUtils>();
			}
			return m_Instance;
		}
	}

	public void Awake() {
		cameraRedBlur = GameObject.Find ("CameraContainer/Camera/Redblur").GetComponent<SpriteRenderer> ();
		cameraRedFilter = GameObject.Find ("CameraContainer/Camera/redFilter").GetComponent<SpriteRenderer> ();
			
		m_Instance = this;
	}


	void Update () {

		if (blink) {
			Debug.Log ("Camera blink");
			Color aColor = cameraRedBlur.color;
			if (increment == true) {    
				aColor.a += opac * Time.deltaTime / blinkLen;

			} else {
				aColor.a -= opac * Time.deltaTime / blinkLen;
			}

			if (aColor.a > opac) {
				increment = false;
			} else if (aColor.a < 0f) {
				blink = false;
			}

			cameraRedBlur.color = aColor;
		}
	}

	public void blinkCamera() {
		regeneration = false;
		Instance.blink = true;
		increment = true;
		Color aColor = cameraRedBlur.color;
		aColor.a = 0f;
		cameraRedBlur.color = aColor;
	}

	public void changeOpecity(float ope) {
		Color aColor = cameraRedFilter.color;
		aColor.a = ope;
		cameraRedFilter.color = aColor;
	}
		

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraUtils : MonoBehaviour {


	public SpriteRenderer cameraRedBlur;
	public bool blink = true;
	public bool increment = true;
	public float opac = 0.8f;
	public float blinkLen = 0.3f;

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

	private void Awake() {
		DontDestroyOnLoad(this);
		m_Instance = this;
	}
		

	private void Start() {
		cameraRedBlur = GameObject.Find ("CameraContainer/Camera/Redblur").GetComponent<SpriteRenderer> ();
	}

	void Update () {

//		if (alarm)
//		{        
//			Color aColor = cameraRedBlur.color;
//			if (increment == true) 
//			{    
//				aColor.a += opac*Time.deltaTime/blurLen;
//
//			} else
//			{
//				aColor.a -= opac*Time.deltaTime/blurLen;
//			}
//
//			if (aColor.a > opac) 
//			{
//				increment = false;
//			} else if (aColor.a < 0f)
//			{
//				increment = true;
//			}
//
//			cameraRedBlur.color = aColor;
//		}

		if (blink) {
			Color aColor = cameraRedBlur.color;
			if (increment == true) 
			{    
				aColor.a += opac*Time.deltaTime/blinkLen;

			} else
			{
				aColor.a -= opac*Time.deltaTime/blinkLen;
			}

			if (aColor.a > opac) 
			{
				increment = false;
			} else if (aColor.a < 0f)
			{
				increment = true;
				blink = false;
			}

			cameraRedBlur.color = aColor;
		}
	}

	public void blinkCamera() {
		Instance.blink = true;
		Color aColor = cameraRedBlur.color;
		aColor.a = 0;
		cameraRedBlur.color = aColor;
		increment = true;


	}

}

using UnityEngine;
using System.Collections;

public class playMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Animator> ().Play ("start_movie");
	}

	public void endMovie() {
		AutoFade.FadeOut(Color.black, moveToFirstLevel);
	}

	public void moveToFirstLevel() {
		Application.LoadLevel("level1");	
	}

	// Update is called once per frame
	void Update () {
	
	}
}

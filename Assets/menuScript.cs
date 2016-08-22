using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {


	public void quitGame() {
		Application.Quit();
	}	

	public void startGame() {
		AutoFade.FadeOut(Color.black, moveToFirstLevel);
	}

	public void moveToFirstLevel() {
		Application.LoadLevel(1);	
	}
}

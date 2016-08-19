using UnityEngine;
using System.Collections;

public class endLevel : MonoBehaviour {

	public int levelToLoad = 1;

	public void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.tag.Equals("Player"))
		{
			AutoFade.FadeOut(Color.black, loadNextLevel);
		}
	}


	public void loadNextLevel() {
		Application.LoadLevel(levelToLoad);	
	}	
	

	

}

using UnityEngine;
using System.Collections;

public class playerDeathLogic : MonoBehaviour {

	public Vector3 respawnTo;
	private int currentCheckpointNum = -1;
	public bool dying = false;

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("checkpoint")) {
			int checkpointNum = other.GetComponent<checkpoint> ().checkpointNum;
			if (checkpointNum > currentCheckpointNum) {
				currentCheckpointNum = checkpointNum;
				respawnTo = other.transform.position;
			}
		}
	} 

	public void die() {
		dying = true;
		// TODO: active death animation.

		AutoFade.FadeOut (Color.black, moveCharPosition);		
	}

	private void moveCharPosition() {
		this.transform.position = respawnTo;
		dying = false;
		AutoFade.FadeIn (null);
	}
}

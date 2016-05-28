using UnityEngine;
using System.Collections;

public class playerDeathLogic : MonoBehaviour {

	public Vector3 respawnTo;
	private int currentCheckpointNum = -1;
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
		AutoFade.FadeOut (Color.black, moveCharPosition);		
	}

	private void moveCharPosition() {
		this.transform.position = respawnTo;
		AutoFade.FadeIn (null);
	}
}

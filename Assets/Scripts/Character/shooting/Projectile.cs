using UnityEngine;
public class Projectile : MonoBehaviour {


	private int force;

	public AudioClip hit;
	public AudioClip breaking;

	public GameObject explosion;

	void Start () {


	}

	void OnCollisionEnter2D(Collision2D collided) {

		Debug.Log ("Collided magic");
		if (collided.gameObject.tag != "Player") {
			//audio.PlayOneShot(breaking);

		}
		if (collided.gameObject.tag == "Enemy") {
			//audio.PlayOneShot(hit);

		}

		if (collided.gameObject.tag != "Enemy") {
			//audio.PlayOneShot(hit);

		}
		destoryObject ();

	}
	public void setForce (int force)
	{
		this.force = force;
	}
		

	private void destoryObject()
	{
		GameObject expl = Instantiate (explosion);
		expl.transform.position = transform.position;
		Destroy (this.gameObject);
	}

}
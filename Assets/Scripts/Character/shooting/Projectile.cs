using UnityEngine;
public class Projectile : MonoBehaviour {


	private int force;

	public AudioClip hit;
	public AudioClip breaking;
	private AudioSource source;
	public GameObject explosion;

	void Start () {
		source = GetComponent<AudioSource>();

	}

	void OnCollisionEnter2D(Collision2D collided) {

		Debug.Log ("Collided magic");
		if (collided.gameObject.tag != "Player") {
			

		}
		if (collided.gameObject.tag == "Enemy") {
			

		}

		if (collided.gameObject.tag != "Enemy") {
			//audio.PlayOneShot(hit);

		}

		source.PlayOneShot(hit, 0.5f);
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
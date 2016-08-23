using UnityEngine;
public class Projectile : MonoBehaviour {


	private int force;

	public AudioClip hit;
	public AudioClip breaking;
	private AudioSource source;

	public GameObject explosion;
	private CollisionFacade collisionFacade; // This is used for the collisions.

	void Start () {
		source = GetComponent<AudioSource>();
		collisionFacade = new CollisionFacade();
	}

	void OnCollisionEnter2D(Collision2D collided) {

		Debug.Log ("Collided magic");
		if (collided.gameObject.tag != "Player") {
			

		}
		if (collided.gameObject.tag == "Enemy") {
			

		}

		if (collided.gameObject.tag == "Enemy" || collided.gameObject.tag == "Boss") {
			collisionFacade.Collision(collided.gameObject, this.gameObject);
		}

		source.PlayOneShot(hit, 0.5f);
		destoryObject ();

	}
	public void setForce (int force)
	{
		this.force = force;
	}

	public int getForce() {
		return this.force;
	}
		

	private void destoryObject()
	{
		GameObject expl = Instantiate (explosion);
		expl.transform.position = transform.position;
		Destroy (this.gameObject);
	}

}
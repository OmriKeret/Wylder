using UnityEngine;
using System.Collections;
using PC2D;
using Com.LuisPedroFonseca.ProCamera2D;

public class shooting : MonoBehaviour {


	private float timeLoading;
	private bool _chargingShot;
	private bool _clickShooting;
	private PlatformerMotor2D _motor;
	private PlatformerAnimation2D _animation;

	public GameObject chargeGlow;
	public Rigidbody2D bullet;
	public float maxSpeed = 300;

	public GameObject explosion;


	// Use this for initialization
	void Start () {
		_motor = GetComponent<PlatformerMotor2D>();
		_animation = GetComponent<PlatformerAnimation2D>();
	}

	// Update is called once per frame
	void Update () {
		_clickShooting = UnityEngine.Input.GetButton("Shoot");

		if (_clickShooting) {
			_animation.chargeShot ();
			// Charging the shot.
			timeLoading += Time.deltaTime;
			_chargingShot = true;
			chargeGlow.SetActive (true);
		} else if (_chargingShot && timeLoading > 2f) {
			timeLoading = 0;
			chargeGlow.SetActive (false);
			_chargingShot = false;
			// We charged the shot, now we shoot.
			shoot ();
		} else if (_chargingShot && timeLoading > 0){
			
			// Animate idle.
			timeLoading = 0;
			_chargingShot = false;
			_animation.setIdle ();
			chargeGlow.SetActive (false);
		}
	}

	private void shoot() {
		_animation.shootShot ();
		ProCamera2DShake.Instance.ShakeUsingPreset("Shot");
		Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 1))) as Rigidbody2D;

		if (_motor.facingLeft) {
			// Shoot left.
			bulletInstance.AddForce (new Vector2 (-1,0) * maxSpeed );
		} else {
			// Shoot right.
			bulletInstance.AddForce (new Vector2 (1,0) * maxSpeed );
		}



	}
}

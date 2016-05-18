using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using EZCameraShake;

public class HeroCombat : MonoBehaviour {

	private PlatformerCharacter2D characterMovment;
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool currentlyAttacking = false;

	private void Awake()
	{
		characterMovment = GetComponent<PlatformerCharacter2D>();
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
		

	public void attack() 
	{
		m_Anim.SetTrigger ("Attack");
		this.GetComponent<playerDeathLogic> ().die ();
		//AutoFade.FadeOut (Color.black, callback);
		//CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
	}

	public void callback() {
		AutoFade.FadeIn(null);
	}
	public void setAttacking() {
		currentlyAttacking = true;
	}

	public void unsetAttacking() {
		currentlyAttacking = false;
	}
}

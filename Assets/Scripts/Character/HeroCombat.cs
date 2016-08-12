using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using EZCameraShake;
using System.Collections.Generic;
using PC2D;

public class HeroCombat : MonoBehaviour, ICharCollider {

	private PlatformerCharacter2D characterMovment;
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private playerDeathLogic deathLogic;
	private PlayerStatsLogic playerStats;
	private PlatformerAnimation2D animationLogic;


	//Player state.
	private eCharState playerState;


	private void Awake()
	{
		characterMovment = GetComponent<PlatformerCharacter2D>();
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		deathLogic = GetComponent<playerDeathLogic> ();
		playerStats = GetComponent<PlayerStatsLogic> ();
		animationLogic = GetComponent<PlatformerAnimation2D> ();
	}

	// Mainly used to synchornize player animation and state.
	void Update() 
	{
		string currentAnimation = animationLogic.GetCurrentAnimatorStateName ();
		if (deathLogic.dying) {
			playerState = eCharState.dead;
		} else if(currentAnimation.StartsWith ("Attack")) {
			playerState = eCharState.Attacking;
		} else if (currentAnimation.StartsWith ("Counter")) {
			playerState = eCharState.Countering;
		} else if (currentAnimation.StartsWith ("Block")) {
			playerState = eCharState.Blocking;
		} else if (currentAnimation.StartsWith ("Hit")) {
			playerState = eCharState.UnderAttack;
		} else if (currentAnimation.StartsWith ("Killing")) {
			playerState = eCharState.KillingMove;
		} else {
			playerState = eCharState.Default;
		}
	}




	public bool isAttacking() {
		return animationLogic.isAttacking ();
	}

	public void attack() 
	{
		// In case player can't attack then return.
		if (playerState == eCharState.Countering || playerState == eCharState.KillingMove || playerState == eCharState.UnderAttack) {
			return;
		}

		playerState = eCharState.Attacking;
		animationLogic.attack ();
//		this.GetComponent<playerDeathLogic> ().die ();
		//AutoFade.FadeOut (Color.black, callback);
		//CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
	}
		

	public void counter() 
	{
		if (playerState == eCharState.Default || playerState == eCharState.Attacking || playerState == eCharState.Blocking) {

			playerState = eCharState.Countering;
			animationLogic.Counter ();
		}

	}

	public void block() 
	{
		// In case player can't attack then return.
		if (playerState == eCharState.Countering || playerState == eCharState.KillingMove || playerState == eCharState.UnderAttack) {
			return;
		}

		playerState = eCharState.Blocking;
		animationLogic.Block ();
		//		this.GetComponent<playerDeathLogic> ().die ();
		//AutoFade.FadeOut (Color.black, callback);
		//CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
	}

	public void counterSuccess() 
	{
		m_Anim.SetTrigger ("Killing");
		playerState = eCharState.KillingMove;
	}

	public void callback() {
		AutoFade.FadeIn(null);
	}



	#region ICharCollider implementation

	public eCharState GetState ()
	{
		return playerState;
	}


	public void ActiveHitAnimation ()
	{
		// No need to implement as Hit take care of it all.

	}

	public void ActiveDeathAnimation ()
	{
		deathLogic.die ();
	}

	public void ActiveHitSound ()
	{
		throw new System.NotImplementedException ();
	}

	public void ActiveDeathSound ()
	{
		// No implementation needed. deathLogic take care of it all.
	}

	public bool Hit (int dmg)
	{
		Debug.Log ("Player got hit!");
		CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
		CameraUtils.Instance.blinkCamera();
		animationLogic.Hit ();
		playerState = eCharState.UnderAttack;
		if (playerStats.hit (dmg)) {
			deathLogic.die ();
			return true;
		}
		return false;


	}

	// The meaning here is active killing move animation, as the counter succeeded
	public void ActiveCounterAnimation ()
	{
		Debug.Log ("Player countered successfully!");
		counterSuccess ();
	}

	#endregion

	/*
	 * Method used by player controller in order to know when to block input
	 * */
	public bool IsStablizie() 
	{
		if (playerState == eCharState.Attacking) {
			return true;
		}
		return false;
	}

	public int getAttackStrength() {
		// TODO: get the stats from somewhere.
		return 1;
	}

}

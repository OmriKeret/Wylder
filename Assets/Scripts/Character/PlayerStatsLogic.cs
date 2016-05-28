﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatsLogic : MonoBehaviour {
	// Player basic stats.
	public float maxHP;
    public float HP = 10;
    public int Strength = 1;

	// Reginartion
	private float lastTimeHitted = 0;
	public float timeToStartGeneration = 3f;
	public float timeToGenerateFromEmptyToFull = 5f;
	private float timeToFullHealthFromCurrentHealth = 5f;
	private float t = 0;
    //GUI
    HPLogic healthLogic;
    StaminaBarLogic staminaBar;


	// Use this for initialization
    void OnEnable()
    {
		maxHP = HP;
		healthLogic = this.GetComponent<HPLogic>();
     //   staminaBar = this.GetComponent<StaminaBarLogic>();
	}


	void Start() 
	{
		maxHP = HP;	
	//	staminaBar.setMaximumStamina(3);
	}

	void Update() {
		if (Time.time - lastTimeHitted >= timeToStartGeneration) {
			t += Time.deltaTime / timeToFullHealthFromCurrentHealth;
			HP = Mathf.Lerp(HP, maxHP, t);

		}
	}

	public bool hit(int force) {
		lastTimeHitted = Time.time;

		timeToFullHealthFromCurrentHealth = (1 - (float)(HP / maxHP)) * timeToGenerateFromEmptyToFull;
		Debug.Log ("time to full health is:" + timeToFullHealthFromCurrentHealth);
		t = 0;
		HP = HP - force;
		Debug.Log("lost health, current Health: " + HP);
		if (HP > 0) {
			healthLogic.reduceHpTo (HP);
			return true;
		} else {
			// Player is dead.
			healthLogic.reduceHpTo (HP);
			return false;
		}
	}
	

}

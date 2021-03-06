﻿using UnityEngine;
using System.Collections;

public interface ICharCollider {

    eCharState GetState();
    void ActiveHitAnimation();
    void ActiveDeathAnimation();
    void ActiveHitSound();
    void ActiveDeathSound();
    bool Hit(int dmg); //(return true if the hitted char is dead after the hit)
	bool RecivePendingDamage(int dmg);
	bool CanCurrentlyAttack(GameObject o);
	void ActiveCounterAnimation(string type);// (we need to think how to synchronize both on the animation)
	int getAttackStrength();
	void dodge();


	/**
	 * The ICharCollider should take care for activating animation and stuff.
	 * After considering it again, we should have the following method only:
	 * GetState()
	 * Hit(int dmg)
	 * ActiveCounter()
	 * getAttackStrength()
	 * */



}

using UnityEngine;
using System.Collections;

public interface ICharCollider {

    eCharState GetState();
    void ActiveHitAnimation();
    void ActiveDeathAnimation();
    void ActiveHitSound();
    void ActiveDeathSound();
    bool Hit(int dmg); //(return true if the hitted char is dead after the hit)
    void ActiveCounterAnimation();// (we need to think how to synchronize both on the animation)

}

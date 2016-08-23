using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour, ICharCollider {

    protected eCharState state;
    protected Dictionary<PlatformerMotor2D.MotorState,Action> moveSM;
    protected PlatformerMotor2D _motor;

    protected Animator animator;
    protected EnemyAnimationHolder animHolder;
    protected PlatformerMotor2D.MotorState priorState;
    protected System.Reflection.MethodInfo changeMotorState;
    protected AudioSource soundSource;


    //States
    protected bool isGrounded;

    //Counters
    protected bool halt = false;
    protected int walkingCounter = 0;
	protected int pendingDemage = 0;

	//Combat.
    public int HP;
    public int AttackForce;
    public int AttackCount;
	protected bool alreadyHitWithinCurrentAttack;

    public float movement { get; private set; }
    public float distanceCheckForSearch;
    public float distanceCheckForJump;
    public float distanceCheckForAttack;
    public List<LineCastModel> lineCastVectors;
    public Vector2 jumpMin;
    public Vector2 jumpMax;
    public Vector2 player;
    protected bool isLanding;

    public LayerMask jumpMask;
    public LayerMask playerMask;
    public LayerMask emptyMask = 0;
    public LayerMask fullMask = 0xFFFF;

    protected GameObject playerObj;
    protected eCharState combatSM;
    protected HashSet<Enemy> nearbyEnemies;
    private static System.Random rnd = new System.Random();

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    public virtual void Start()
    {

        animHolder = GetComponent<EnemyAnimationHolder>();
        soundSource = GetComponent<AudioSource>();
        _motor = GetComponent<PlatformerMotor2D>();
        changeMotorState = typeof(PlatformerMotor2D).GetProperty("motorState").GetSetMethod(true);
        
        movement = -1;

        moveSM = new Dictionary<PlatformerMotor2D.MotorState, Action> {
            {PlatformerMotor2D.MotorState.OnGround,GroundRaycast },
            {PlatformerMotor2D.MotorState.Jumping,Jump },
            {PlatformerMotor2D.MotorState.Falling, Air }
        };

        playerObj = FindObjectOfType<PlayerController2D>().gameObject;

        lineCastVectors = new List<LineCastModel>
        {
            new LineCastModel() {MainObject = transform, Start = player, TargetObject = playerObj.transform, Size = distanceCheckForSearch , Invoker = ChasePlayer, Mask = fullMask},
            new LineCastModel() {MainObject = transform, Start = jumpMin, End = Vector2.right, Size = distanceCheckForJump, Invoker = Jump, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = jumpMax, End = Vector2.right, Size = distanceCheckForJump, Invoker = Flip, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = player, End = Vector2.right, Size = distanceCheckForAttack, Invoker = Attack, Mask = playerMask},
            new LineCastModel() {MainObject = transform, Start = new Vector2(), End = new Vector2(), Size = 1 , Invoker = Rest, Mask = 0},
            new LineCastModel() {MainObject = transform, Start = new Vector2(), End = new Vector2(), Size = 1 , Invoker = ActiveDeathAnimation, Mask = 0},
        };

        nearbyEnemies = new HashSet<Enemy>();
        combatSM = eCharState.Default;
    }

    //protected virtual void Update()
    //{
    //    if (!halt)
    //    {
    //        if (inCombat)
    //        {

    //        }
    //        else
    //        {
    //            Raycastion();
    //            Behaviours();
    //            UpdateAnimation();
    //        }
    //    }
    //}

    protected virtual void FixedUpdate()
    {
		Debug.Log ("Enemy state is: " + combatSM.ToString());

		// If the enemy is dead or under attack, no use for raycasting.
		if (combatSM == eCharState.dead || combatSM == eCharState.UnderAttack || combatSM == eCharState.Attacking) {
			return;
		}
		Debug.Log ("Enemy state is: " + combatSM.ToString());
        if (moveSM.Keys.Contains(_motor.motorState))
        {
            moveSM[_motor.motorState].Invoke();
        }
			
        else
        {
            Debug.Log("Missing " + _motor.motorState);
        }

        priorState = _motor.motorState;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {

    }

    private static int _lockFlag = 0; // 0 - free


    protected void ChooseAttacker()
    {
        
    }

    protected abstract void Attack();

    protected abstract void Idle();

    protected virtual void Flip()
    {

        Debug.Log("Flip");
        if (movement < 0)
            transform.rotation = new Quaternion(0, 180f, 0, 1);
        else
            transform.rotation = new Quaternion(0, 0, 0, 1);
        Vector3 scale = transform.localScale;
        scale += new Vector3(0, 0, -2 * scale.z);
        transform.localScale = scale;
        movement *= -1;
        lineCastVectors.ForEach((lineCaster) => lineCaster.Flip());

    }

    protected virtual void GroundRaycast()
    {
        _motor.fallFast = false;
        lineCastVectors.ForEach((lineCaster) => lineCaster.Cast(false));
        lineCastVectors.Reverse();	
        LineCastModel mostRelevantHit = null;
        int hitCount = 0;
        
        lineCastVectors.ForEach((lineCaster) =>
        {
            if (lineCaster.CheckAndReset() && mostRelevantHit == null)
            {
                hitCount++;
                mostRelevantHit = lineCaster;
            }
        });
        lineCastVectors.Reverse();
        if (mostRelevantHit == null) //No hit means the way is clear
            Patrol();
        else
            mostRelevantHit.Invoker.Invoke();
    }

    protected virtual void Patrol()
    {

        _motor.normalizedXMovement = movement;
        animator.Play(animHolder.Run);        
    }

    protected virtual bool SyncAttacks()
    {
        foreach (Enemy enemy in nearbyEnemies)
        {
            if (enemy.GetState() == eCharState.Attacking)
            {
                Rest();
                return false;
            }
        }

        int totalProbability = 0;
        Dictionary<Enemy, int> candidates = new Dictionary<Enemy, int>();
        foreach (Enemy enemy in nearbyEnemies)
        {
            enemy.lineCastVectors[(int)eLineCaster.searchPlayer].Cast(false);
            if (!enemy.lineCastVectors[(int)eLineCaster.searchPlayer].CheckAndReset())
                continue;
            int currentProb = enemy.AttackProbability();
            candidates.Add(enemy, currentProb);
            totalProbability += currentProb;
        }
        if (totalProbability == 0)
            return true;
        //Debug.Log("Should attack out of " + totalProbability);
        
        int attacker = rnd.Next(totalProbability);
        foreach (Enemy enemy in candidates.Keys)
        {
            totalProbability -= candidates[enemy];
            if (totalProbability <= 0)
            {
                //enemy.ChangeCombatState(eCharState.Attacking);
            }
            else
            {
                enemy.Rest();
            }
        }
        return combatSM != eCharState.Default;

    }

    public int AttackProbability()
    {
        return HP + AttackCount + AttackForce;
    }

    IEnumerator FlipAndWait(int seconds)
    {
        halt = true;
        yield return new WaitForSeconds(seconds);
        Flip();
        yield return new WaitForSeconds(seconds);
        halt = false;
    }

    protected virtual void ChasePlayer()
    {
        bool faceLeft = movement < 0;
        bool enemyOnLeft = playerObj.transform.position.x < transform.position.x;
        Debug.Log(string.Format("Chase {0}, {1}", faceLeft, enemyOnLeft));
        if (faceLeft ^ enemyOnLeft)
        {
            Flip();
        }
        Patrol();
    }

    protected void Jump()
    {
        _motor.Jump();
        animator.Play(animHolder.Jump);
    }

    protected void Air()
    {
        changeMotorState.Invoke(_motor, new object[] { PlatformerMotor2D.MotorState.Falling });
        Debug.Log("Air");
        animator.Play(animHolder.Air);
    }

    protected void Land()
    {
        Debug.Log("Land");
    }

    protected void RestOrSleep(string action)
    {
        lineCastVectors[(int)eLineCaster.rest].Mask = fullMask;
        ChangeCombatState(eCharState.Default);
        _motor.normalizedXMovement = 0;
        animator.Play(action);
        Debug.Log(action);
    }

    protected void Rest()//one iteration
    {
        RestOrSleep(animHolder.Idle);
    }

    protected void Sleep()//repeate until wakeup
    {
        RestOrSleep(animHolder.Idle);
    }

    protected void WakeUp()
    {
        lineCastVectors[(int)eLineCaster.rest].Mask = 0;
        Debug.Log("WakeUp");
    }

    protected void NowLanding(bool land)
    {
        isLanding = land;
    }

    public eCharState GetState()
    {
		return combatSM;
    }

    public void ChangeCombatState(eCharState state)
    {
        combatSM = state;
    }

    public void ActiveHitAnimation()
    {
        animator.Play(animHolder.RecieveHit);
    }

    public void ActiveDeathAnimation()
    {
        state = eCharState.dead;
		ChangeCombatState (eCharState.dead);
        lineCastVectors[(int)eLineCaster.death].Mask = fullMask;
        _motor.normalizedXMovement = 0;
        animator.Play(animHolder.Death);
    }

    public abstract void ActiveHitSound();

    public abstract void ActiveDeathSound();

    protected void Dead()
    {
        animator.enabled = false;
        Destroy(_motor);
        Destroy(this.gameObject, 2);
    }

    public bool Hit(int dmg)
    {
        HP -= dmg;
        bool isDead = HP <= 0;
		if (isDead) {
			ActiveDeathAnimation ();
			state = eCharState.dead;
			ChangeCombatState (eCharState.dead);
		} else {
			ChangeCombatState (eCharState.UnderAttack);
			ActiveHitAnimation ();
		}
            
        return isDead;
    }

	public void ActiveCounterAnimation(String type)
    {
        animator.Play(animHolder.Counter);
    }

    public int getAttackStrength()
    {
        return AttackForce;
    }

    public void addEnemyNearby(Enemy other)
    {
        if (!other.nearbyEnemies.Contains(this))
        {
            other.Sleep();
            nearbyEnemies.Add(other);
        }
    }

    public void removeEnemyNearby(Enemy other)
    {
        if (nearbyEnemies.Contains(other))
        {
            nearbyEnemies.Remove(other);
            other.WakeUp();
        }
        Debug.Log("Remove");
        
    }

    protected enum eLineCaster
    {
        searchPlayer = 0,
        minJump,
        maxJump,
        attackPlayer,
        rest,
        death
    }

	public bool RecivePendingDamage(int dmg) {
		pendingDemage = dmg;
		return HP - dmg <= 0;
	}

	public void TakePendingDamage() {
		Hit (pendingDemage);
		pendingDemage = 0;
	}

	public virtual bool CanCurrentlyAttack(GameObject o) {
		var beforeChange = alreadyHitWithinCurrentAttack;
		alreadyHitWithinCurrentAttack = true;
		return !beforeChange;
	}

	public void dodge() {
	}


}

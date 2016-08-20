using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour, ICharCollider {

    protected const int MAX_HP = 5;
    protected const int UNREACHABLE = 0, CLIMB = 1, JUMP = 2, FLOOR = 3;
    protected Vector3 raysDelta;

    protected int currentHp;
    protected Rigidbody2D _rigidbody;
    protected bool inCombat;
    protected eCharState state;
    protected Dictionary<PlatformerMotor2D.MotorState,Action> moveSM;
    protected PlatformerMotor2D _motor;

    protected List<Transform> rays;
    protected List<bool> raysCurrentHit;
    public bool m_FacingRight = false;
    float distToGround;
    protected int layerMask;
    protected int currentFPJ;

    protected Animator animator;
    protected PlatformerMotor2D.MotorState priorState;
    protected System.Reflection.MethodInfo changeMotorState;

    //States
    protected bool isGrounded;

    //Counters
    protected bool halt = false;
    protected int walkingCounter = 0;


    public int HP;
    public int AttackForce;

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
    public LayerMask restMask = 0xFFFF;

    protected GameObject playerObj;
    protected eCharState combatSM;

    public virtual void Awake()
    {
        currentHp = MAX_HP;
        animator = GetComponent<Animator>();
        
    }

    public virtual void Start()
    {

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
            new LineCastModel() {MainObject = transform, Start = player, TargetObject = playerObj.transform, Size = distanceCheckForSearch , Invoker = Patrol, Mask = playerMask},
            new LineCastModel() {MainObject = transform, Start = jumpMin, End = Vector2.right, Size = distanceCheckForJump, Invoker = Jump, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = jumpMax, End = Vector2.right, Size = distanceCheckForJump, Invoker = Flip, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = player, End = Vector2.right, Size = distanceCheckForAttack, Invoker = Attack, Mask = playerMask},
            new LineCastModel() {MainObject = transform, Start = new Vector2(), End = new Vector2(), Size = 1 , Invoker = Rest, Mask = 0},
        };

        
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

    IEnumerator FlipAndWait(int seconds)
    {
        halt = true;
        yield return new WaitForSeconds(seconds);
        Flip();
        yield return new WaitForSeconds(seconds);
        halt = false;
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
        animator.Play("Run");
        
    }

    protected void Jump()
    {
        _motor.Jump();
        animator.Play("Jump");
    }

    protected void Air()
    {
        changeMotorState.Invoke(_motor, new object[] { PlatformerMotor2D.MotorState.Falling });
        Debug.Log("Air");
        animator.Play("Air");
    }

    protected void Land()
    {
        Debug.Log("Land");
    }

    protected void Rest()
    {
        lineCastVectors[(int)eLineCaster.rest].Mask = restMask;
        _motor.normalizedXMovement = 0;
        animator.Play("IDLE");
        Debug.Log("Rest");
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
        animator.Play("Recieve_Hit");
    }

    public void ActiveDeathAnimation()
    {
        animator.Play("Death");
    }

    public void ActiveHitSound()
    {
        throw new NotImplementedException();
    }

    public void ActiveDeathSound()
    {
        throw new NotImplementedException();
    }

    public bool Hit(int dmg)
    {
        currentHp -= dmg;
        Debug.Log("Hit enemy");
        return currentHp > 0;
    }

	public void ActiveCounterAnimation(String type)
    {
        throw new NotImplementedException();
    }

    public int getAttackStrength()
    {
        return AttackForce;
    }

    protected enum eLineCaster
    {
        searchPlayer = 0,
        minJump,
        maxJump,
        attackPlayer,
        rest
    }
}

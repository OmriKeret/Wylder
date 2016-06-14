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

    public GameObject raysCollection;
    public bool FaceRight;
    [Range(0, 10)]
    public float raySize;
    [Range (0,100)]
    public float MaxVelocity;
    [Range(0, 100)]
    public float RunForce;
    [Range(0, 100)]
    public float ClimbMaxSpeed;
    [Range(0, 100)]
    public float JumpForce;
    [Range(0, 10)]
    public int FramePerJump;

    protected List<Transform> rays;
    protected List<bool> raysCurrentHit;
    protected bool m_FacingRight = true;
    float distToGround;
    protected int layerMask;
    protected int currentFPJ;

    protected Animator animator;

    //States
    protected bool isGrounded;

    //Counters
    protected bool halt = false;
    protected int walkingCounter = 0;

    public virtual void Awake()
    {
        currentHp = MAX_HP;
        animator = GetComponent<Animator>();
        raysDelta = new Vector3(raySize, 0, 0);
    }

    public virtual void Start()
    {
        rays = raysCollection.GetComponentsInChildren<Transform>().Where(child=> child != raysCollection.transform).ToList<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        layerMask = ~(1 << LayerMask.NameToLayer("bones")) & ~(1 << LayerMask.NameToLayer("camera")) & ~(1 << LayerMask.NameToLayer("PlayerExternal"));
        distToGround = rays[FLOOR].transform.position.y;
        currentFPJ = 0;
        if (!FaceRight)
            raysDelta *= -1;
    }

    protected virtual void Update()
    {
        if (!halt)
        {
            if (inCombat)
            {

            }
            else
            {
                Raycastion();
                Behaviours();
                UpdateAnimation();
            }
        }
    }

    protected void Raycastion()
    {
        raysCurrentHit = new List<bool>();
        rays.ForEach(ray =>
        {
            Debug.DrawLine(ray.position, ray.position + raysDelta, Color.blue);
            RaycastHit2D g = Physics2D.Linecast(ray.position, ray.position + raysDelta, layerMask);
            raysCurrentHit.Add(g);
        });

        Debug.DrawLine(rays[FLOOR].position, (Vector2)rays[FLOOR].position - Vector2.up * 0.1f , Color.blue);
        isGrounded = Physics2D.Linecast(transform.position, (Vector2)rays[FLOOR].position - Vector2.up * 0.1f, layerMask);

        if (currentFPJ > 0)
            currentFPJ--;
    }

    protected void Behaviours()
    {
        Patrol();
    }

    private void UpdateAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("InCombat", false);
        animator.SetFloat("Velocity", Mathf.Abs(_rigidbody.velocity.x));
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

    protected abstract int Attack();

    protected abstract void Idle();

    protected virtual void Move(float velocity)
    {
        Move(velocity , 0);
    }

    protected virtual void Move(float xv, float yv)
    {
        if (!isGrounded)
        {
            return;
        }

        xv *= m_FacingRight ? 1 : -1; //Flip velocity
        Vector2 speed = new Vector2(xv, yv);
        if (_rigidbody.velocity.magnitude <= MaxVelocity)
        {
            _rigidbody.AddForce(speed, ForceMode2D.Force);
        }
    }

    //protected virtual void Climb()
    //{
    //    isClimbing = true;
    //    if (_rigidbody.velocity.y <= ClimbMaxSpeed)
    //    {
    //        _rigidbody.AddForce(Vector2.up* ClimbMaxSpeed, ForceMode2D.Force);
    //    }
        
    //}

    protected virtual void Jump(float force)
    {
        if (!isGrounded || currentFPJ > 0)
        {
            return;
        }
        _rigidbody.AddForce(new Vector2(0,force), ForceMode2D.Impulse);
        currentFPJ = FramePerJump;
    }

    protected virtual void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        raysDelta *= -1;
    }

    void turnAround()
    {
        float speed = _rigidbody.velocity.x;
        if (speed > 0.5f)
        {
            Move(0.25f * speed);
        }
        else
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            StartCoroutine(FlipAndWait(2));
        }
        
    }

    protected virtual void Patrol()
    {
        if (raysCurrentHit[UNREACHABLE])
        {
            turnAround();
            Debug.Log(rays[UNREACHABLE].name);
        }
        else if (raysCurrentHit[JUMP])
        {
            Jump(JumpForce);
        }
        else if (raysCurrentHit[FLOOR])
        {
            Move(RunForce*3, RunForce*3);
        }
        else
        {
            Move(RunForce);
        }
    }

    public eCharState GetState()
    {
        throw new NotImplementedException();
    }

    public void ActiveHitAnimation()
    {
        animator.SetTrigger("GotHit");
    }

    public void ActiveDeathAnimation()
    {
        animator.SetTrigger("Death");
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
        return currentHp > 0;
    }

    public void ActiveCounterAnimation()
    {
        throw new NotImplementedException();
    }

    public int getAttackStrength()
    {
        throw new NotImplementedException();
    }
}

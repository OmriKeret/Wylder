using UnityEngine;
using System.Collections.Generic;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;
		public Animator m_ControlAnimator; // TO be deleted if we dont use a ragdoll
        private PlatformerMotor2D _motor;
        private Animator _animator;
        private bool _isJumping;
        private bool _currentFacingLeft;
		private HeroCombat heroCombat;

		//Name table for animation state.
		public string[] AnimatorStateNames = {"Jump","Land", "RunStop", "Slide", "Run", "Walk", "Roll", "Attack", "Walk__Combat", "Idle_Combat", "runAttack"};
		private Dictionary<int, string> NameTable { get; set; }
	


        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PlatformerMotor2D>();
            _animator = visualChild.GetComponent<Animator>();
            _animator.Play("Idle");
			heroCombat = GetComponent<HeroCombat>();
            _motor.onJump += SetCurrentFacingLeft;
			BuildNameTable();
        }

        // Update is called once per frame
        void Update()
        {
			 if (isAttacking () && _motor.motorState != PlatformerMotor2D.MotorState.Dashing) {
				return;
			}
            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.Play("Jump");

                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {
                _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;

                if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
                    _animator.Play("Fall");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
                         _motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
                {
                    _animator.Play("Cling");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
                {
                    _animator.Play("On Corner");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
                {
					_animator.Play("Slide");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    _animator.Play("Dash");
                }
                else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
						float speed = _motor.velocity.magnitude;
						if (_motor.sprint) {
							_animator.Play ("Run");
						} else if (speed > _motor.groundSpeed && !_motor.onSlope || speed > _motor.sprintSpeed) {
							_animator.Play("Slide");
						} else {
							_animator.Play("Walk");
						}
                        
                    }
					else 
                    {
                        _animator.Play("Idle");
                    }

                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                _motor.motorState == PlatformerMotor2D.MotorState.Dashing ||
                _motor.motorState == PlatformerMotor2D.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }
            
            if (Mathf.Abs(valueCheck) >= 0.1f)
            {
                Vector3 newScale = visualChild.transform.localScale;
                newScale.x = Mathf.Abs(newScale.x) * ((valueCheck > 0) ? 1.0f : -1.0f);

				// remove this part
				if (newScale.x > 0) { 
					m_ControlAnimator.SetBool ("flip", false);
				} else {
					m_ControlAnimator.SetBool ("flip", true);
				}
				// up till here when removing the regdoll and uncomment the next line
                //visualChild.transform.localScale = newScale;
            }
        }

		public void attack() {
			_animator.Play ("Attack");
		}

		public void Counter() {
			_animator.Play ("Counter");
		}

		public void Block() {
			_animator.Play ("Block");
		}

		public void Hit() {
			_animator.Play ("Hit");
		}

		public void KillingMove() {
			_animator.Play ("Hit");
		}


        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }

		private void BuildNameTable()
		{
			NameTable = new Dictionary<int, string>();

			foreach (string stateName in AnimatorStateNames)
			{
				
				NameTable[Animator.StringToHash(stateName)] = stateName;
			}
		}

		public string GetCurrentAnimatorStateName()
		{
			AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

			string stateName;
			if (NameTable.TryGetValue(stateInfo.shortNameHash, out stateName))
			{
				return stateName;
			}
			else
			{
				Debug.LogWarning("Unknown animator state name.");
				return string.Empty;
			}
		}

		public bool isAttacking() {
			return GetCurrentAnimatorStateName ().StartsWith ("Attack") ;
		}
    }


}

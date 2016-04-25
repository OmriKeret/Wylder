using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof (PlatformerCharacter2D))]
	[RequireComponent(typeof (HeroCombat))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
		private HeroCombat m_Combat;
        private bool m_Jump;
		private bool m_Run;
		private bool m_Attack;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
			m_Combat = GetComponent<HeroCombat>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			if (!m_Attack) 
			{
				// Read the attack input in Update so button presses aren't missed.
				m_Attack = CrossPlatformInputManager.GetButtonDown("Attack");
			}
				
				
        }


        private void FixedUpdate()
        {
            // Read the inputs.
			bool crouch = Input.GetKey(KeyCode.DownArrow);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
			m_Run = CrossPlatformInputManager.GetButton("Run");
			m_Character.Move(h, crouch, m_Jump, m_Run);

			if (m_Attack) 
			{
				m_Combat.attack ();
				m_Attack = false;
			}
            m_Jump = false;
		
        }
    }
}

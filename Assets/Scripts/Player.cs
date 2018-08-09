using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Config
    [SerializeField] float m_runSpeed = 5f;
    [SerializeField] float m_jumpSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    
    // Messages
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
	}
	
	void Update () {
        Run();
        Jump();
        FlipSprite();
	}

    // Methods
    void Run ()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");   // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * m_runSpeed, m_rigidbody.velocity.y);
        m_rigidbody.velocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(m_rigidbody.velocity.x) > Mathf.Epsilon;
        m_animator.SetBool("Running", hasHorizontalSpeed);
    }

    void FlipSprite ()
    {
        bool hasHorizontalSpeed = Mathf.Abs(m_rigidbody.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(m_rigidbody.velocity.x), 1f);
        }
    }

    void Jump ()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 velocityToAdd = new Vector2(0f, m_jumpSpeed);
            m_rigidbody.velocity += velocityToAdd;
        }
    }
}

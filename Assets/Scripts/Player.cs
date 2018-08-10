using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Config
    [SerializeField] float m_runSpeed = 5f;
    [SerializeField] float m_jumpSpeed = 5f;
    [SerializeField] float m_climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float feetOnGroundDist = .6f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    CapsuleCollider2D m_bodyCollider;
    BoxCollider2D m_feetCollider;
    float gravityScaleAtStart;
    int groundLayer;
    int climbingLayer;

    // Messages
    void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_bodyCollider = GetComponent<CapsuleCollider2D>();
        m_feetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = m_rigidbody.gravityScale;
        groundLayer = LayerMask.GetMask("Ground");
        climbingLayer = LayerMask.GetMask("Climbing");
    }
	
	void Update () {
        if (!isAlive) return;

        Run();
        Jump();
        FlipSprite();
        Climb();
        Die();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, feetOnGroundDist, groundLayer);
        if (hit.collider != null)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 velocityToAdd = new Vector2(0f, m_jumpSpeed);
                m_rigidbody.velocity += velocityToAdd;
            }
        }
    }

    void Climb ()
    {
        if (m_bodyCollider.IsTouchingLayers(climbingLayer))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(m_rigidbody.velocity.x, controlThrow * m_climbSpeed);
            m_rigidbody.velocity = climbVelocity;
            m_rigidbody.gravityScale = 0f;

            bool hasVerticalSpeed = Mathf.Abs(m_rigidbody.velocity.y) > Mathf.Epsilon;
            m_animator.SetBool("Climbing", hasVerticalSpeed);
        }
        else
        {
            m_animator.SetBool("Climbing", false);
            m_rigidbody.gravityScale = gravityScaleAtStart;
            return;
        }        
    }

    void Die ()
    {
        if (m_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            m_animator.SetTrigger("Dying");
            m_rigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}

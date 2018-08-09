using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Config
    [SerializeField] float m_runSpeed = 5f;
    [SerializeField] float m_jumpSpeed = 5f;
    [SerializeField] float m_climbSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    CapsuleCollider2D m_bodyCollider;
    BoxCollider2D m_feetCollider;
    float gravityScaleAtStart;

    // Messages
    void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_bodyCollider = GetComponent<CapsuleCollider2D>();
        m_feetCollider = GetComponent<BoxCollider2D>();
        print(m_feetCollider);

        gravityScaleAtStart = m_rigidbody.gravityScale;
	}
	
	void Update () {
        Run();
        Jump();
        FlipSprite();
        Climb();
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
        // TODO: use a downward raycast to check if ground is below the player
        if (!m_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 velocityToAdd = new Vector2(0f, m_jumpSpeed);
            m_rigidbody.velocity += velocityToAdd;
        }
    }

    void Climb ()
    {
        if (!m_feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            m_animator.SetBool("Climbing", false);
            m_rigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(m_rigidbody.velocity.x, controlThrow * m_climbSpeed);
        m_rigidbody.velocity = climbVelocity;
        m_rigidbody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(m_rigidbody.velocity.y) > Mathf.Epsilon;
        m_animator.SetBool("Climbing", hasVerticalSpeed);
    }
}

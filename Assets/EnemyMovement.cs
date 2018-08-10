using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D m_rigidbody;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsFacingRight())
        {
            m_rigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            m_rigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
	}

    void OnTriggerExit2D ()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(m_rigidbody.velocity.x)), 1f);
    }

    bool IsFacingRight ()
    {
        return transform.localScale.x > 0;
    }
}

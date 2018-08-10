using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSfx;

    int points = 1;

    void OnTriggerEnter2D (Collider2D other)
    {
        AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
        Destroy(gameObject);
        FindObjectOfType<GameSession>().AddScore(points);
    }
}

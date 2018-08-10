using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] int score;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    void Awake ()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

	// Use this for initialization
	void Start () {
		
	}

    public void AddScore (int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath ()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void TakeLife ()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void ResetGameSession ()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}

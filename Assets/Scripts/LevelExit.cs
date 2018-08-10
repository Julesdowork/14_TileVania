using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] float slowmoFactor = 0.2f;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>().IsAlive())
            StartCoroutine(AdvanceLevel());
    }

    IEnumerator AdvanceLevel()
    {
        Time.timeScale = slowmoFactor;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

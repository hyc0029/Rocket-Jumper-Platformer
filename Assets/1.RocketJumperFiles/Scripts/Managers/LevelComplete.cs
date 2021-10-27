using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComplete : MonoBehaviour
{

    [SerializeField] private float timeBeforeWinScreen;
    [SerializeField] private float fadeInTime;
    private CanvasGroup LevelCompleteScreen;

    void Awake()
    {
        LevelCompleteScreen = GameObject.FindGameObjectWithTag("LevelComplete").GetComponent<CanvasGroup>();
        LevelCompleteScreen.alpha = 0;
    }

    private void Update()
    {
        if (LevelCompleteScreen.alpha == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }

    IEnumerator LevelCompleteFade()
    {
        FindObjectOfType<CharacterController>().myCol.enabled = false;
        FindObjectOfType<CharacterController>().playerRigidbody.velocity = new Vector2 (0.4f, 0.6f) * 50;
        yield return new WaitForSeconds(timeBeforeWinScreen);
        
        float timer = 0;
        while (timer < fadeInTime)
        {
            LevelCompleteScreen.alpha = timer / fadeInTime;
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }
        LevelCompleteScreen.alpha = 1;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<CharacterController>())
            StartCoroutine(LevelCompleteFade());
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private CanvasGroup pauseCanvas;
    private RocketLauncherControl rlc;
    private CharacterController cc;
    [SerializeField] private GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = GetComponent<CanvasGroup>();
        pauseCanvas.alpha = 0;
        rlc = FindObjectOfType<RocketLauncherControl>();
        cc = FindObjectOfType<CharacterController>();
        buttons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && cc.myCol.enabled)
        {
            if (pauseCanvas.alpha == 0)
            {
                pauseCanvas.alpha = 1;
                buttons.SetActive(true);
                Time.timeScale = 0;
                rlc.enabled = false;
            }
            else
            {
                resumeGame();
            }
        }
    }

    public void resumeGame()
    {
        pauseCanvas.alpha = 0;
        Time.timeScale = 1;
        rlc.enabled = true;
        buttons.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}

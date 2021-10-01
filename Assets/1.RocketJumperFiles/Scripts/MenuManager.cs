using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject setting;

    // Start is called before the first frame update
    void Start()
    {
        pause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            pauseAndUnpauseGame();
    }


    public void pauseAndUnpauseGame()
    {
        pause.SetActive(!pause.activeSelf);
        pauseGame(pause.activeSelf);
    }

    public void settingMenu()
    {
        setting.SetActive(!setting.activeSelf);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    void pauseGame(bool paused)
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

}

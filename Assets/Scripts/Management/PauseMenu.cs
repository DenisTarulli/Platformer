using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;

    [HideInInspector] public bool gameIsPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.gameIsOver)
        {
            if (gameIsPaused)
            {
                BackSound();
                Resume();
            }
            else
                Pause();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;

        pauseMenuUI.SetActive(true);
        //AudioManager.instance.Play("Pause");
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        //AudioManager.instance.Play("BackUI");
        Application.Quit();
    }

    public void ConfirmSound()
    {
        //AudioManager.instance.Play("ClickUI");
    }

    public void BackSound()
    {
        //AudioManager.instance.Play("BackUI");
    }
}

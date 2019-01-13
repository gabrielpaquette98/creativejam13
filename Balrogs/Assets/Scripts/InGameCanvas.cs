using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                DisplayPausePanel();
            }
            else if (isPaused)
            {
                ClosePausePanel();
            }
        }
    }

    public void DisplayPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
}

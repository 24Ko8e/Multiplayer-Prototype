using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreboard;

    PlayerController Controller;
    public void setPlayerController(PlayerController _controller)
    {
        Controller = _controller;
    }

    private void Start()
    {
        PauseMenu.isPaused = false;
    }

    private void Update()
    {
        SetFuelAmount(Controller.GetFuelAmount);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isPaused = pauseMenu.activeSelf;

    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
}

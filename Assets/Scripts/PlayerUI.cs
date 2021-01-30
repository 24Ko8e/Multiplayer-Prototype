﻿using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    GameObject pauseMenu;

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

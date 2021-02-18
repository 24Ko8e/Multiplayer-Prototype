using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;
    [SerializeField]
    RectTransform healthBarFill;
    [SerializeField]
    Text ammoText;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreboard;

    Player player;
    PlayerController Controller;
    WeaponManager weaponManager;
    public void setPlayer(Player _player)
    {
        player = _player;
        Controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    private void Start()
    {
        PauseMenu.isPaused = false;
    }

    private void Update()
    {
        SetFuelAmount(Controller.GetFuelAmount);
        SetHealthAmount(player.GetHealthPercentage());
        SetAmmoAmount(weaponManager.getCurrentWeapon.currentBullets);

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

    private void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }

    private void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
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

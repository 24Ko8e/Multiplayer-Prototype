using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;

    PlayerController Controller;
    public void setPlayerController(PlayerController _controller)
    {
        Controller = _controller;
    }

    private void Update()
    {
        SetFuelAmount(Controller.GetFuelAmount);
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
}

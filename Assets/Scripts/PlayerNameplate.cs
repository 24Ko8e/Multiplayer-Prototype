using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour
{
    [SerializeField]
    Text usernameText;

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    Player player;

    Camera m_Camera;

    private void Start()
    {
    }

    void Update()
    {
        m_Camera = Camera.main;
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);

        usernameText.text = player.username;
        healthBarFill.localScale = new Vector3(player.GetHealthPercentage(), 1f, 1f);
    }
}

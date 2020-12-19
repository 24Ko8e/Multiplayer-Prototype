using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayer = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    GameObject playerUI_inst;

    Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayer));

            playerUI_inst = Instantiate(playerUIPrefab);
        }

        GetComponent<Player>().Setup();
    }

    private void SetLayerRecursively(GameObject playerGraphics, int newLayer)
    {
        playerGraphics.layer = newLayer;

        foreach (Transform child in playerGraphics.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.registerPlayer(_netID, _player);
    }

    private void OnDisable()
    {
        Destroy(playerUI_inst);

        if (sceneCamera)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.unregisterPlayer(transform.name);
    }
}

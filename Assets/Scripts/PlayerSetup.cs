using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
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
    [HideInInspector]
    public GameObject playerUI_inst;

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
            //Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayer));

            playerUI_inst = Instantiate(playerUIPrefab);
            playerUI_inst.name = playerUIPrefab.name;
            PlayerUI ui = playerUI_inst.GetComponent<PlayerUI>();
            
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component on PlayerUI Prefab!");
            }

            ui.setPlayerController(GetComponent<PlayerController>());

            GetComponent<Player>().SetupPlayer();
        }
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

        if(isLocalPlayer)
            GameManager.instance.setSceneCameraState(true);
        
        GameManager.unregisterPlayer(transform.name);
    }
}

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

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
        if (sceneCamera)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.unregisterPlayer(transform.name);
    }
}

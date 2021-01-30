using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    uint roomSize = 4;
    string roomName;
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void CreateRoom()
    {
        if (roomName != null && roomName != "")
        {
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }

    public void disableBtn(Button btn)
    {
        btn.enabled = false;
    }
}

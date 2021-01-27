using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;

public class JoinGameManager : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();
    [SerializeField]
    Text status;
    [SerializeField]
    Transform roomListParent;
    [SerializeField]
    GameObject roomListItemPrefab;
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager != null)
        {
            networkManager.StartMatchMaker();
        }
        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Searching for rooms...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";

        if (!success || matches == null)
        {
            status.text = "Failed to fetch room list.";
            return;
        }

        foreach (MatchInfoSnapshot match in matches)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);
            RoomListItem _roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }
            roomList.Add(roomListItemGO);
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms found!";
        }
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining room...";
    }

    private void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

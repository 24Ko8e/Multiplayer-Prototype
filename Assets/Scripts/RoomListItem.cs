using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    Text roomName;
    [SerializeField]
    Text roomSize;
    MatchInfoSnapshot matchInfo;

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    JoinRoomDelegate joinRoomCallback;

    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        matchInfo = _match;
        joinRoomCallback = _joinRoomCallback;

        roomName.text = matchInfo.name;
        roomSize.text = "(" + matchInfo.currentSize + "/" + matchInfo.maxSize + ")";
    }

    public void JoinRoom()
    {
        joinRoomCallback.Invoke(matchInfo);
    }
}

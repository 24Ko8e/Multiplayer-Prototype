using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    //private void OnDestroy()
    //{
    //    if (player != null)
    //        SyncScore();
    //}

    void SyncScore()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetUserData(OnDataReceived);
    }

    void OnDataReceived(string data)
    {
        if (player.kills == 0 && player.deaths == 0)
            return;

        int kills = DataTranslator.retrieveKillCount(data);
        int deaths = DataTranslator.retrieveDeathCount(data);

        int newKills = player.kills + kills;
        int newDeaths = player.deaths + deaths;

        string newData = DataTranslator.encodeToData(newKills, newDeaths);

        player.kills = 0;
        player.deaths = 0;

        Debug.Log("Starting sync: " + newData);
        UserAccountManager.instance.SetUserData(newData);
    }
}

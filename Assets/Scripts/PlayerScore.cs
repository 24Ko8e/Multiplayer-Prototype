using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour
{
    int lastKills = 0;
    int lastDeaths = 0;

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
        if (player.kills <= lastKills && player.deaths <= lastDeaths)
            return;

        int killsSinceLastSync = player.kills - lastKills;
        int deathsSinceLastSync = player.deaths - lastDeaths;

        int kills = DataTranslator.retrieveKillCount(data);
        int deaths = DataTranslator.retrieveDeathCount(data);

        int newKills = killsSinceLastSync + kills;
        int newDeaths = deathsSinceLastSync + deaths;

        string newData = DataTranslator.encodeToData(newKills, newDeaths);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        Debug.Log("Starting sync: " + newData);
        UserAccountManager.instance.SetUserData(newData);
    }
}

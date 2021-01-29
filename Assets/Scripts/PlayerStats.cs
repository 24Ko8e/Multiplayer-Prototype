using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text killCount;
    public Text deathCount;

    void Start()
    {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetUserData(OnDataRetrieved);
    }
    
    void OnDataRetrieved(string data)
    {
        killCount.text = DataTranslator.retrieveKillCount(data).ToString() + " KILLS";
        deathCount.text = DataTranslator.retrieveDeathCount(data).ToString() + " DEATHS";
    }
}

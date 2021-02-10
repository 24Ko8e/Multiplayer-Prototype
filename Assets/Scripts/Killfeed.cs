using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeed : MonoBehaviour
{
    [SerializeField]
    GameObject killfeddItemPrefab;

    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string playerKilled, string source)
    {
        GameObject GO = (GameObject)Instantiate(killfeddItemPrefab, this.transform);
        GO.GetComponent<KillfeedItem>().Setup(playerKilled, source);
        Destroy(GO, 4f);
    }

    void Update()
    {
        
    }
}

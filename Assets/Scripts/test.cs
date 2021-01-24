using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("OnEnable executed at " + Time.time);
    }
    private void Awake()
    {
        Debug.Log("Awake executed at " + Time.time);
    }

    void Start()
    {
        Debug.Log("Start executed at " + Time.time);
    }

    void Update()
    {
        
    }
}

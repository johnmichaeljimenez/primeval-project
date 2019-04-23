using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval;

public class GameManager : GenericSingletonClass<GameManager> {

    public string t;
    public static float gameTime
    {
        get{
            float t = 0;

            if (NetworkClient.isConnected)
            {
                t = (float)(NetworkTime.time-startTime);
            }

            return t;
        }
    }
    
    public static float startTime {get; private set;}

    public void InitializeTime()
    {
        //TODO: synchronize time to clients
        startTime = (float)NetworkTime.time;
    }
    
    public override void Awake()
    {
        base.Awake();
    }

    public void StartGame()
    {
        InitializeTime();
        ItemSpawner.instance.SpawnItems();
    }

    void Update()
    {
        
    }
}

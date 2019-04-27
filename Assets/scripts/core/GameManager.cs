using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval.Networking;
using Primeval.PlayerCharacter;

public class GameManager : GenericSingletonClass<GameManager>
{

    public string t;
    public static float gameTime;

    public static float startTime { get; private set; }

    public void InitializeTime()
    {
        //TODO: synchronize time to clients
        startTime = (float)NetworkTime.time;
        gameTime = 0;
    }

    public void UpdateTime()
    {
        float t = 0;

        if (NetworkClient.isConnected)
        {
            t = (float)(NetworkTime.time - startTime);
                PlayerCharacter.myPlayer.CmdGameTime(t);
        }
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
        if (NetworkManagerExt.isHosting)
            UpdateTime();
    }

    public static void DeployPlayer()
    {
        PlayerCharacter.myPlayer.deploymentModule.Deploy(Vector2.zero);//TODO: move to lobby phase
    }
}

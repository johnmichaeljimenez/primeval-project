using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;

public class Outpost : MonoBehaviour
{
    public void Initialize()
    {

    }

    public void OnUpdate()
    {

    }


    public void DropFuel()
    {
        PlayerCharacter.myPlayer.inventoryModule.DropFuel();
    }
}

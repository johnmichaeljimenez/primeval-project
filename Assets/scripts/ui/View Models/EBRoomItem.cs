using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using Primeval.Data;
using Primeval.PlayerCharacter;

[Binding]
public class EBRoomItem : MonoBehaviour
{
    [Binding]
    public void ClickItem()
    {
        //TODO: join room
        //PlayerCharacter.myPlayer.inventoryModule.UseItem(transform.GetSiblingIndex());
    }
}

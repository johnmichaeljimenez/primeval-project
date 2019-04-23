using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using Primeval.Data;
using Primeval.PlayerCharacter;

[Binding]
public class EBInventoryItem : MonoBehaviour
{
    [Binding]
    public void ClickItem()
    {
        PlayerCharacter.myPlayer.inventoryModule.UseItem(transform.GetSiblingIndex());
    }
}

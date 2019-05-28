using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using Primeval.Networking;
using Primeval.ViewModels;

[Binding]
public class EBRoomItem : MonoBehaviour
{
    [Binding]
    public void ClickItem()
    {
        print("joining room");
        //TODO: join room
        VMRoomItem v =  (VMRoomItem)GetComponent<Template>().GetViewModel();
        NetworkManagerExt.JoinRoom(v.RoomName);
        //PlayerCharacter.myPlayer.inventoryModule.UseItem(transform.GetSiblingIndex());
    }
}

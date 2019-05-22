﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Primeval.Item;

public class Interactable : Photon.MonoBehaviour {

	public void OnInteract(GameObject g)
	{
		ItemBase i = GetComponentInParent<ItemBase>();
		PlayerCharacter p = GetComponentInParent<PlayerCharacter>();

		if (i)
		{
			print("pick up item");
			i.OnPickup(g);
		}else if (p)
		{
			// print("open inventory");
			// p.inventoryModule.OpenInventory(g.GetComponentInParent<PlayerCharacter>());
		}
	}

}

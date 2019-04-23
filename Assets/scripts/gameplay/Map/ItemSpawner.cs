using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval.PlayerCharacter;

public class ItemSpawner : GenericSingletonClass<ItemSpawner>
{
    public Dictionary<string, GameObject> itemList;

    public GameObject[] itemPrefabs;
    public Transform itemSpawnPointSet;

    void Awake()
    {
        base.Awake();
        
        itemList = new Dictionary<string, GameObject>();
        foreach (GameObject i in itemPrefabs)
        {
            itemList.Add(i.gameObject.name, i);
        }
    }

    public void SpawnItems()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject n = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Transform t = itemSpawnPointSet.GetChild(Random.Range(0, itemSpawnPointSet.childCount));

            Vector3 tp = Random.onUnitSphere*0.8f;

            GameObject g = Instantiate(n, t.position+tp, Quaternion.identity);

            NetworkServer.Spawn(g);//, PlayerCharacter.hostPlayer.gameObject);
        }
    }

    public void DropItem(string n)
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject item_prefab;
    public SnappingPoint snapping_point;
    public bool spawnOnStart;

    public void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        GameObject spawned_object = Instantiate(item_prefab);
        spawned_object.GetComponent<InteractableWorking>().Snap(snapping_point);
    }
}

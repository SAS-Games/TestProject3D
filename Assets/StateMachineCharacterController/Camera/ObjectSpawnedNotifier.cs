using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawnedNotifier : MonoBehaviour
{
    private void Start()
    {
        var listeners = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IObjectSpawnedListener>();
        foreach (var listener in listeners)
            listener.OnSpawn(gameObject);
    }
}

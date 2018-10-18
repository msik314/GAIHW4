using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CGScript : MonoBehaviour
{
    Transform[] birds;

    void Start()
    {
        birds = FindObjectsOfType<FlockScript>().Select(o => o.transform).ToArray();
    }

    void Update()
    {
        Vector2 position = Vector2.zero;
        foreach (var b in birds)
        {
            position += (Vector2)b.position;
        }
        position /= birds.Length;
        transform.position = position;
    }
}
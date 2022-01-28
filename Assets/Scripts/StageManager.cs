using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    MapManager mapManager;

    private void OnValidate()
    {
        mapManager = gameObject.GetComponent<MapManager>() == null ? gameObject.AddComponent<MapManager>() : gameObject.GetComponent<MapManager>();
    }
    private void Start()
    {
        
    }
}

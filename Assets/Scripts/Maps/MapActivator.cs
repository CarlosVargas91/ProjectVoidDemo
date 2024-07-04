using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] private string mapToActivate;
    // Start is called before the first frame update
    void Start()
    {
        MapController.instance.ActivateMap(mapToActivate);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagniteScript : MonoBehaviour
{
    public GameObject iron;
    public GameObject magnite;
    void Start()
    {
        
    }

    void Update()
    {
        if (magnite != null)
        {
            Vector3 magnitePosition = magnite.transform.position;
            iron.transform.position = magnitePosition;
        }
    }
}

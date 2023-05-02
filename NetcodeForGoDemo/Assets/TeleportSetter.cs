using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportSetter : MonoBehaviour
{
    [SerializeField] TeleportationArea TeleportationArea = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TeleportationArea.interactionManager == null)
        {
            TeleportationArea.interactionManager = GameObject.FindGameObjectWithTag("player").GetComponent<XRInteractionManager>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVrSimController : MonoBehaviour
{

    [SerializeField] GameObject vrSim;
    [SerializeField] bool spawnSim;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if(spawnSim && Application.isEditor)
        {
            Instantiate(vrSim, transform);
        }
    }
}

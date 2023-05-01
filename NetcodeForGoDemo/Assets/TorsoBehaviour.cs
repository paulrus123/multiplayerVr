using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TorsoBehaviour : NetworkBehaviour
{
    [SerializeField] private Transform head;

    // Update is called once per frame
    void Update()
    {
        if (IsClient && !IsOwner) return;

        Vector3 position = new Vector3(head.position.x, head.position.y - 0.6f, head.position.z);
        transform.position = position;
    }
}

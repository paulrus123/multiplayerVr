using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetwork : NetworkBehaviour
{
    private Vector2 placementArea = new Vector2(-10f, 10f);
    [SerializeField] private GameObject avatarHead;

    public override void OnNetworkSpawn()
    {
        DisableClientInptut();
    }

    private void DisableClientInptut()
    {
        if(IsClient && !IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientMoveProvider.enableInputActions = false;
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead.enabled = false;
            
            foreach (var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }

            avatarHead.SetActive(true);
        }
        else
        {
            avatarHead.SetActive(false);
        }
    }

    private void Start()
    {
        if(IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), transform.position.y, Random.Range(placementArea.x, placementArea.y));
        }
     }
}

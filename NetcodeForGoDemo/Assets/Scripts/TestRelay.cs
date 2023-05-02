using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRKeys;

public class TestRelay : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private XRSimpleInteractable hostInteractable;
    [SerializeField] private XRSimpleInteractable clientInteractable;

    [SerializeField] private Keyboard keyboard;

    public static string JoinCode { get; private set; }

    // Start is called before the first frame update
    async void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        keyboard.Enable();
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += Instance_SignedIn;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostInteractable.selectEntered.AddListener((XRSimpleInteractable) =>
        {
            StartCoroutine(LoadYourAsyncScene(0));
        });

        clientInteractable.selectEntered.AddListener((XRSimpleInteractable) =>
        {
            StartCoroutine(LoadYourAsyncScene(2));
        });

        hostBtn.onClick.AddListener(() =>
        {
            Debug.Log("Host clicked");
            StartCoroutine(LoadYourAsyncScene(0));
        });

        serverBtn.onClick.AddListener(() =>
        {
            StartCoroutine(LoadYourAsyncScene(1));
        });

        clientBtn.onClick.AddListener(() =>
        {
            StartCoroutine(LoadYourAsyncScene(2));
        });


    }

    private void Instance_SignedIn()
    {
        Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
    }

    IEnumerator LoadYourAsyncScene(int mode) // 0 = host, 1 = server, 2 = client
    {
        Debug.Log("LoadingScene");
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("XRScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        switch (mode)
        {
            case 0:
                CreateRelay();
                break;
            case 2:
            default:
                JoinRelay(keyboard.text);
                break;
        }
    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(8);

            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join Code: " + JoinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async void JoinRelay(string code)
    {
        JoinCode = code;
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }
}

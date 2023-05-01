using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkManagerUi : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private XRSimpleInteractable hostInteractable;
    [SerializeField] private XRSimpleInteractable clientInteractable;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

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
                NetworkManager.Singleton.StartHost();
                break;

            case 1:
                NetworkManager.Singleton.StartServer();
                break;
            case 2:
            default:
                NetworkManager.Singleton.StartClient();
                break;
        }

    }

}

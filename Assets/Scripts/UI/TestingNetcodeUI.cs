using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;


    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            KitchenGameMultiplayer.Instance.StartHost();
            Hide();
        });
        clientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            KitchenGameMultiplayer.Instance.StartClient();
            Hide();
        });
    }


    private void Hide() => gameObject.SetActive(false);
}

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.ShowOptionsUI();
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePaused += GameHandler_OnLocalGamePaused;
        KitchenGameManager.Instance.OnLocalGameUnPaused += GameHandler_OnLocalUnGamePaused;

        Hide();
    }

    private void GameHandler_OnLocalUnGamePaused(object sender, System.EventArgs e) => Hide();

    private void GameHandler_OnLocalGamePaused(object sender, System.EventArgs e) => Show();

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}

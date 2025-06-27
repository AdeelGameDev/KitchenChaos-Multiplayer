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
            Gamemanager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.ShowOptionsUI();
        });
    }

    private void Start()
    {
        Gamemanager.Instance.OnGamePaused += GameHandler_OnGamePaused;
        Gamemanager.Instance.OnGameUnPaused += GameHandler_OnUnGamePaused;

        Hide();
    }

    private void GameHandler_OnUnGamePaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameHandler_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

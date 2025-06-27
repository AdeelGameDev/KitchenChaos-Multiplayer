using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance;

    [SerializeField] private Transform pressAnyKeyToRebindTransform;

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private Button moveUPButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button inteactButton;
    [SerializeField] private Button inteactAlternateButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;


    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            HideOptionsUI();
        });

        moveUPButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_UP);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_DOWN);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_LEFT);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_RIGHT);
        });
        inteactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });
        inteactAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteactAlternate);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });


    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGameUnPaused += Gamemanager_OnGameUnPaused;
        UpdateVisual();
        HidePressAnyKeyToRebind();
        HideOptionsUI();
    }

    private void Gamemanager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        HideOptionsUI();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects :" + Mathf.Round(AudioManager.Instance.GetVolume() * 10);
        musicText.text = "Music :" + Mathf.Round(MusicManager.Instance.GetVolume() * 10);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_UP);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_DOWN);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_LEFT);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_RIGHT);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteactAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void ShowOptionsUI()
    {
        gameObject.SetActive(true);
    }

    private void HideOptionsUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressAnyKeyToRebind()
    {
        pressAnyKeyToRebindTransform.gameObject.SetActive(true);
    }
    private void HidePressAnyKeyToRebind()
    {
        pressAnyKeyToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressAnyKeyToRebind();

        GameInput.Instance.RebindBinging(binding, () =>
        {
            HidePressAnyKeyToRebind();
            UpdateVisual();
        });
    }

}

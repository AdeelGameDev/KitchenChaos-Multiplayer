using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;

    private void Start()
    {
        UpdateVisual();
        Show();
        Gamemanager.Instance.OnStateChanged += Gamemanager_OnStateChanged;
        GameInput.Instance.OnBindingRebind += Gameinput_OnBindingRebind;
    }

    private void Gamemanager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(Gamemanager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void Gameinput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_UP);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_DOWN);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_LEFT);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_RIGHT);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteactAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

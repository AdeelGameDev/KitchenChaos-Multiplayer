using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class CountDownTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousNumber;
    private const string NUMBER_POPUP_ANIM = "NumberPopup";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Gamemanager.Instance.OnStateChanged += GameState_OnStateChange;
        Hide();
    }

    private void Update()
    {
        int countNumber = Mathf.CeilToInt(Gamemanager.Instance.GetCountDownToStartTimer());
        countDownText.text = countNumber.ToString();

        if (previousNumber != countNumber)
        {
            previousNumber = countNumber;
            animator.SetTrigger(NUMBER_POPUP_ANIM);
            AudioManager.Instance.PlayPopupSound();
        }
    }

    private void GameState_OnStateChange(object sender, EventArgs e)
    {
        if (Gamemanager.Instance.IsCountDownToStartActive())
            Show();
        else
            Hide();
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}

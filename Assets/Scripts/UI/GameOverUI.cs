using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;


    private void Start()
    {
        Gamemanager.Instance.OnStateChanged += GameState_OnStateChange;
        Hide();
    }


    private void GameState_OnStateChange(object sender, EventArgs e)
    {
        if (Gamemanager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessRecipesCount().ToString();
        }
        else
            Hide();
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}

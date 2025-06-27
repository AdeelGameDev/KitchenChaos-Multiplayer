using UnityEngine;
using UnityEngine.UI;

public class GamePlayTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;


    private void Update()
    {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayTimerNormalized();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI resultText;
    [Space]
    [SerializeField] private Color deliverySuccessColor;
    [SerializeField] private Color deliveryFailedColor;
    [Space]
    [SerializeField] private Sprite deliverySuccesSprite;
    [SerializeField] private Sprite deliveryFailedSprite;
    [Space]
    private Animator animator;
    private const string POPUP_ANIM = "Popup";


    //-----------------------------------------------------

    private void Awake() => animator = GetComponent<Animator>();

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;

        gameObject.SetActive(false);
    }

    //-----------------------------------------------------

    private void DeliveryManager_OnRecipeFail(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP_ANIM);
        backgroundImage.color = deliveryFailedColor;
        iconImage.sprite = deliveryFailedSprite;
        resultText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP_ANIM);
        backgroundImage.color = deliverySuccessColor;
        iconImage.sprite = deliverySuccesSprite;
        resultText.text = "DELIVERY\nSUCCESS";
    }
}

using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [SerializeField] private Player playerComp;

    private Animator playerAnimator;
    private const string IS_WALKING = "IsWalking";

    private void Awake() => playerAnimator = GetComponent<Animator>();

    private void Update() => playerAnimator.SetBool(IS_WALKING, playerComp.IsWalking);
}

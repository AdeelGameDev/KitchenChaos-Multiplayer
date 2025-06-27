using Unity.Netcode;
using UnityEngine;

public class PlayerVisuals : NetworkBehaviour
{
    [SerializeField] private Player player;
    [Space]
    private Animator playerAnimator;
    private const string IS_WALKING = "IsWalking";

    //--------------------------------------------------

    private void Awake() => playerAnimator = GetComponent<Animator>();

    private void Update()
    {
        if (!IsOwner) return;

        playerAnimator.SetBool(IS_WALKING, player.IsWalking);
    }
}

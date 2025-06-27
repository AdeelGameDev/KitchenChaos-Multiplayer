using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private Player player;

    private float footstepTimer;
    private float footstepTimerMax = .1f;


    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking)
            {
                AudioManager.Instance.PlayFootStepSound(player.transform.position, 1);
            }
        }
    }

}

using System;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] KitchenObjectSO plateObjectSO;

    private float plateSpawnTimer;
    private float plateSpawnTimerMax = 5;

    private float platesSpawnAmount;
    private float platesSpawnAmountMax = 4;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;

        if (plateSpawnTimer >= plateSpawnTimerMax)
        {
            plateSpawnTimer = 0;

            if (Gamemanager.Instance.IsGamePlaying() && platesSpawnAmount < platesSpawnAmountMax)
            {
                // TODO : Spawn Plate visual
                platesSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if (platesSpawnAmount > 0)
            {
                // There is atleast one plate here
                KitchenObject.SpawnKitchenObject(plateObjectSO, player);
                platesSpawnAmount--;

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

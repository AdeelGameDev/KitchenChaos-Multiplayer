using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter
{
    public static event EventHandler onAnyObjectTrashed;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());

            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        onAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }


    public static new void ResetStaticData()
    {
        onAnyObjectTrashed = null;
    }
}

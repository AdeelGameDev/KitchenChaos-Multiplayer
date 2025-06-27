using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler onAnyObjectTrashed;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            onAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

    public static new void ResetStaticData()
    {
        onAnyObjectTrashed = null;
    }
}

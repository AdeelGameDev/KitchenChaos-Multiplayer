using Unity.Netcode;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Has kitchen object
            if (player.HasKitchenObject())
            {
                //Player is carrying kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
            }
        }
        else
        {
            // dont have kitchen object
            if (player.HasKitchenObject())
            {
                // Player is carrying kitchen object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is carrying Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                    }
                }
            }
            else
            {
                // Players is not caryying kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }



}

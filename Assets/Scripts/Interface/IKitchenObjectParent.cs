using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObjectParent(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearObjectParent();

    public bool HasKitchenObject();

    public NetworkObject GetNetworkObject();
}

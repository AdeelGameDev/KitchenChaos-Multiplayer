using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransform;


    private void LateUpdate()
    {
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
    }

    public void SetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }


}

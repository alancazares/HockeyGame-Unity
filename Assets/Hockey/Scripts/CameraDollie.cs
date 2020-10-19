using UnityEngine;

public class CameraDollie : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;
    public Vector3 desiredPosition;

    //Fixed update fixes the camera being jagged, not sure why
    private void FixedUpdate()
    {
        desiredPosition = new Vector3(target.position.x, target.position.y, -41f) + offset;
        Vector3 SmoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = SmoothedPosition;
    }
}

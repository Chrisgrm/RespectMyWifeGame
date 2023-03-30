using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class RaycastController : MonoBehaviour
{
    public const float skinwidht = .15f;
    public const float dstBetweenRays = 25f;
    [HideInInspector]
    public int horizontalRaycount;
    [HideInInspector]
    public int verticalRaycount;
    public LayerMask collisionMask;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    [HideInInspector]
    public BoxCollider collider;
    public RaycastOrigins raycastOrigins;
    public virtual void Start()
    {
        collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidht * -2);

        raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, transform.position.z);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, transform.position.z);
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, transform.position.z);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, transform.position.z);

    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidht * -2);

        float boundsWidht = bounds.size.x;
        float boundsHeight = bounds.size.y;
         
        horizontalRaycount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRaycount = Mathf.RoundToInt(boundsWidht / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRaycount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRaycount - 1);  


    }

    public struct RaycastOrigins
    {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;

    }

}

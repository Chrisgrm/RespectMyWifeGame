﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

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
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;
    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidht * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

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
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

    }

}

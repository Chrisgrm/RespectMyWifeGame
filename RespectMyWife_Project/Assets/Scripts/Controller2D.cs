using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (BoxCollider))]

public class Controller2D : RaycastController
{
    public CollisionInfo collisionInfo;
   
    public override void Start()
    {
        base.Start();

    }
    public void Move(Vector3 velocity)
    {

        UpdateRaycastOrigins();
        collisionInfo.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }       
        
        transform.Translate(velocity);
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinwidht ;
        for (int i = 0; i < verticalRaycount; i++)
        {
            Vector2 rayOrigin = (directionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY*rayLenght, Color.red);
        
            if (hit)
            {
                velocity.y = (hit.distance - skinwidht ) * directionY;
                rayLenght = hit.distance;
                collisionInfo.above = directionY == 1;
                collisionInfo.below = directionY == -1;
            }
        }

    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinwidht;
        for (int i = 0; i < horizontalRaycount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinwidht) * directionX;
                rayLenght = hit.distance;
                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }

    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}

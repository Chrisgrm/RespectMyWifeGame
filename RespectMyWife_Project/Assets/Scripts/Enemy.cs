using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Enemy : MonoBehaviour
{
    public float jumpHigh = 130;
    public float time2JumpMaxHigh = .5f;

    float gravity;
    float moveSpeed = 200;
    float accelerationTimeAirbone = .5f;
    float accelerationTimeGrounded = .2f;
    float jumpVelocity;
    float velocityXSmoothing;    
    Vector3 velocity;
    float enemyNoiseSensibility=260;
    float visionCamp=750;
    float directionX;



    BoxCollider2D collider;
    Controller2D controller;
    public LayerMask collisionMask;
   
    void Start()
    {    
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHigh) / Mathf.Pow(time2JumpMaxHigh, 2);
        jumpVelocity = Mathf.Abs(gravity) * time2JumpMaxHigh;
        velocity = Vector3.left;
        directionX = -1;


    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }      
                
        velocity.y += gravity * Time.deltaTime;        
        VisionRaycast(ref velocity);         
        controller.Move(velocity * Time.deltaTime);
    }


    void VisionRaycast(ref Vector3 velocity)
    {
        directionX = Mathf.Sign(velocity.x);
        float targetVelocityX = moveSpeed * directionX;
        float backDirectionX = -Mathf.Sign(velocity.x);
        float backRayLenght = enemyNoiseSensibility;
        bool isHit=false;
        for (int i = 0; i < controller.horizontalRaycount; i++)
        {
                     
            float rayLenght = visionCamp;
            Vector2 rayOrigin = (directionX == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (controller.horizontalRaySpacing * i) ;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.blue);

            if (hit)
            {
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);
                isHit = true;
            }
            else
            {
                isHit = false;              
            }

            // Backvision Raycast
            Vector2 backRayOrigin = (backDirectionX == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
            backRayOrigin += Vector2.up * (controller.horizontalRaySpacing * i);
            RaycastHit2D backHit = Physics2D.Raycast(backRayOrigin, Vector2.right * backDirectionX, backRayLenght, collisionMask);
            Debug.DrawRay(backRayOrigin, Vector2.right * backDirectionX * backRayLenght, Color.blue);

            if (backHit)
            {
                velocity.x = backDirectionX;

            }

        }
        if(!isHit)velocity.x= Mathf.SmoothDamp(velocity.x, 0, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Atack")
        {

        }

    }




}

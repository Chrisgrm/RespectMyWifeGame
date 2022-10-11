using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Enemy : MonoBehaviour
{
    //Enemy Variables
    protected float jumpHigh;
    protected float time2JumpMaxHigh ;   
    protected float moveSpeed;
    protected float accelerationTimeAirbone ;
    protected float accelerationTimeGrounded ;
    protected float enemyNoiseSensibility;
    protected float visionCamp;
    protected float atackRange;

    //
    protected float gravity;
    protected float jumpVelocity;
    protected float velocityXSmoothing;    
    protected Vector3 velocity;   
    protected float directionX;


    BoxCollider2D collider;
    protected Controller2D controller;
    public LayerMask collisionMask;
   
    public void Start()
    {    
        controller = GetComponent<Controller2D>();
    }

    public void VisionRaycast(ref Vector3 velocity)
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
                if (hit.distance <= atackRange)
                {
                    print("atack");
                }
                
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
                
               
                velocity.x = Mathf.SmoothDamp(velocity.x, 1*backDirectionX, ref velocityXSmoothing, (controller.collisionInfo.below) ? (accelerationTimeGrounded*0.5f) : accelerationTimeAirbone);                    
                

            }

        }
        if(!isHit)velocity.x= Mathf.SmoothDamp(velocity.x, 0, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);
    }

    protected void StandardActions(){

 
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }      
                
        velocity.y += gravity * Time.deltaTime;        
        VisionRaycast(ref velocity);         
        controller.Move(velocity * Time.deltaTime);
    }

    protected void StartConfig(bool initialDirection)
    {
        //calculo de gravedad y velocidad de salto
        gravity = -(2 * jumpHigh) / Mathf.Pow(time2JumpMaxHigh, 2);
        jumpVelocity = Mathf.Abs(gravity) * time2JumpMaxHigh;

        //configuracion de direccion inicial
        if (initialDirection)
        {            
            velocity.x=1;
        }
        else
        {
            velocity.x = -1;           
        }
      

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Atack")
        {

        }

    }




}

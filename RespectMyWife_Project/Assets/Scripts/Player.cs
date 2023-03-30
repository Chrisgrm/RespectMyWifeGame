using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    public float jumpHigh = 130;
    public float time2JumpMaxHigh = .5f;

    float gravity;
    float moveSpeed = 400;
    float accelerationTimeAirbone = .5f;
    float accelerationTimeGrounded = .2f;
    float jumpVelocity;
    float velocityXSmoothing;
    bool canJump;
    Vector3 velocity;
    
    public Weapon weaponEquiped;
    Controller2D controller;
    CapsuleCollider2D attackCollider;
    public Joystick movementJstick;
    public Joystick attackJstick;
    bool haveWeapon=true;    
    public RectTransform collectWeaponBtn;
    public RectTransform releaseWeaponBtn;
    Weapon lastWeaponPased;
    RectTransform attackJstickHandle;
    RectTransform movementJstickHandle;
    public Transform crosshair;
    float firstTouchX = 0;
    public Transform refWpnContainer;
    Animator anim;
    float timeAttackJstick = 0;
    float timeMovementJstick = 0;
    bool onGuard = false;
    





    void Start()
    {
        controller = GetComponent<Controller2D>();
        attackCollider = transform.GetChild(1).GetComponent<CapsuleCollider2D>(); 
        gravity = -(2 * jumpHigh) / Mathf.Pow(time2JumpMaxHigh, 2);
        jumpVelocity = Mathf.Abs(gravity)  * time2JumpMaxHigh;
        attackJstickHandle = attackJstick.transform.GetChild(0).GetComponent<RectTransform>();
        movementJstickHandle = movementJstick.transform.GetChild(0).GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame  
    void Update()
    {
        Touch t;
        Touch t2;
        float targetVelocityX = 0;
        UpdateWeaponPosition(velocity);
        
     
        // reinicio de gravedad
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if ((Input.GetButtonDown("Jump")|| movementJstick.Vertical > .75f) && controller.collisionInfo.below && canJump)
        {
            if (haveWeapon){
                velocity.y = jumpVelocity-weaponEquiped.getWeight();
            }else{
                velocity.y = jumpVelocity;
            }
            
            canJump = false;
        }
        if (movementJstick.Vertical < .2f) canJump = true;

        
        ///movimiento y ataque
        if (Input.touchCount > 0)
        {
            
            t = Input.GetTouch(0);
            if (t.phase.Equals(TouchPhase.Began))
            {
                firstTouchX = t.position.x;
            }
            
            if ((firstTouchX) < 500)
            {
                t.fingerId = 1;              
            }     
                        
            if (Input.touchCount > 1)
            {
                t2 = Input.GetTouch(1);
                if (haveWeapon)
                {
                    targetVelocityX = (input.x + movementJstick.Horizontal) * (moveSpeed - weaponEquiped.getWeight());
                    attackJstickHandle.gameObject.SetActive(false);
                    releaseWeaponBtn.gameObject.SetActive(true);
                    crosshair.gameObject.SetActive(true);

                    if (t.position.x > 500 && t.fingerId == 0)
                    {
                        aimCrosshair(t);
                    }
                    else if (t2.position.x > 500 )
                    {
                        aimCrosshair(t2);
                    }

                }
              
            }
            
            if (haveWeapon)
            {
                targetVelocityX = (input.x + movementJstick.Horizontal) * (moveSpeed - weaponEquiped.getWeight()); // Movimiento
                attackJstickHandle.gameObject.SetActive(false);
                releaseWeaponBtn.gameObject.SetActive(true);
                crosshair.gameObject.SetActive(true);

                if (t.position.x > 500 && t.fingerId==0)
                {
                    aimCrosshair(t);
                }
                  
            }
            else
            {
                if (onGuard)
                {
                   

                    if (movementJstickHandle.gameObject.activeSelf)
                    {                        
                    timeMovementJstick = timeMovementJstick + Time.deltaTime;
                        print(timeMovementJstick);
                        
                        if(timeMovementJstick > 0.2)
                        {                          
                            
                            targetVelocityX = (input.x + movementJstick.Horizontal) * moveSpeed/2; //movimiento
                            if (movementJstick.Vertical != 0 && (movementJstick.Vertical > 0.5f || movementJstick.Vertical < -0.5f))
                            {
                                anim.SetFloat("VerticalEvade", movementJstick.Vertical);
                            }
                        }
                        if (timeMovementJstick < 0.2f)
                        {
                            anim.SetFloat("VerticalEvade", 0);
                      

                            if (movementJstick.Horizontal != 0 && (movementJstick.Horizontal>0.5f || movementJstick.Horizontal < -0.5f))
                            {
                                anim.SetFloat("HorizontalEvade", movementJstick.Horizontal);    
                            }
                           
                        }
                        
                    }
                    else
                    {
                        
                        if (timeMovementJstick < 0.2f && timeMovementJstick!=0)
                        {
                            anim.SetTrigger("Evade");
                            //if (movementJstick.Horizontal<0)
                            //{
                            //    anim.SetTrigger("LeftEvade");
                            //}else if(movementJstick.Horizontal>0)
                            //{
                            //    anim.SetTrigger("RightEvade");
                            //}

                        }
                        timeMovementJstick = 0;
                    }
                    

                }
                else
                {
                    targetVelocityX = (input.x + movementJstick.Horizontal) * moveSpeed; //movimiento
                }
                crosshair.gameObject.SetActive(false);
                
            


                if (attackJstickHandle.gameObject.activeSelf)
                {
                    timeAttackJstick = timeAttackJstick + Time.deltaTime;
                     

                    if (timeAttackJstick > 0.13f)
                    {
                        print("cobertura");
                        onGuard = true;
                        anim.SetBool("OnGuard", true);
                    }
                    else
                    {
                        if (attackJstick.Horizontal < 0)
                        {

                            print("izquierda");

                        }
                        else if (attackJstick.Horizontal > 0)
                        {

                            print("derecha");
                        }

                    }

                }
                else
                {
                    if (timeAttackJstick < 0.1f && timeAttackJstick!=0)
                    {
                        if (attackJstick.Horizontal == 0)
                        {
                            PlayerAttack();
                            
                        }                    

                    }
                    anim.SetBool("OnGuard", false);
                    onGuard = false;
                    timeAttackJstick = 0;
                }
                
                
            }
            
            
            
        }

        if ((movementJstick.Vertical < -.75f))
        {
            anim.SetBool("Crouch", true);
            targetVelocityX = 0;
        }
        else
        {
            anim.SetBool("Crouch", false);
           

        }

        //Velocidad en x
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);

        
    

        //gravedad
        velocity.y += gravity * Time.deltaTime;
      
        controller.Move(velocity * Time.deltaTime);

        

        //Vector3 touch = Input.GetTouch(0).position;
        //print(touch);
        //Camera.main.WorldToScreenPoint(touch);
        
        
        
        
      
    }
    //private void LateUpdate()
    //{
    //    refWpnContainer.up = refWpnContainer.position - crosshair.position;
    //}
    void aimCrosshair(Touch t)
    {
        Vector3 shootDirection =(crosshair.position - weaponEquiped.FirePoint.position).normalized;
        if ((t.phase.Equals(TouchPhase.Stationary)||(t.phase.Equals(TouchPhase.Moved))))
            {

            crosshair.position = Camera.main.ScreenToWorldPoint(new Vector3(
            t.position.x,
            t.position.y,
            ((-Camera.main.transform.position.z)+(transform.position.z))));



            weaponEquiped.WeaponAttack(shootDirection);
        }
    }
    void UpdateWeaponPosition(Vector3 velocity ){
        float directionX = Mathf.Sign(velocity.x);
        float range = 0.62f;      
        attackCollider.offset = new Vector2(directionX*range,0);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            collectWeaponBtn.gameObject.SetActive(true);
            releaseWeaponBtn.gameObject.SetActive(false);
            lastWeaponPased = other.gameObject.GetComponent<Weapon>();

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            collectWeaponBtn.gameObject.SetActive(false);          

        }

    }
    private void PlayerAttack()
    {
        
       
        attackCollider.enabled = true;
        anim.SetTrigger("Punch");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("punch"))
        {
            print("aja");
            anim.SetTrigger("Punch2");
        }
    
    }
    public void CollectWeapon()
    {
        haveWeapon = true;        
        weaponEquiped =
            lastWeaponPased;        
    }
    public void ReleaseWeapon()
    {
        haveWeapon = false;
        releaseWeaponBtn.gameObject.SetActive(false);

    }
      

}

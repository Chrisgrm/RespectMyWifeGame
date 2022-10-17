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
    
    Weapon weaponEquiped;
    Controller2D controller;
    CapsuleCollider2D attackCollider;
    public Joystick movementJstick;
    public Joystick attackJstick;
    bool haveWeapon=false;    
    public RectTransform collectWeaponBtn;
    public RectTransform releaseWeaponBtn;
    Weapon lastWeaponPased;
    RectTransform attackJstickHandle;
    public Transform crosshair;



    void Start()
    {
        controller = GetComponent<Controller2D>();
        attackCollider = transform.GetChild(1).GetComponent<CapsuleCollider2D>(); 
        gravity = -(2 * jumpHigh) / Mathf.Pow(time2JumpMaxHigh, 2);
        jumpVelocity = Mathf.Abs(gravity)  * time2JumpMaxHigh;
        attackJstickHandle = attackJstick.transform.GetChild(0).GetComponent<RectTransform>();
       
    }

    // Update is called once per frame  
    void Update()
    {

        Touch t;
        Touch t2;
        float targetVelocityX ;
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

        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            
            if (Input.touchCount > 1)
            {
                t2 = Input.GetTouch(1);
                if (haveWeapon)
                {
                    targetVelocityX = (input.x + movementJstick.Horizontal) * (moveSpeed - weaponEquiped.getWeight());
                    attackJstickHandle.gameObject.SetActive(false);
                    releaseWeaponBtn.gameObject.SetActive(true);
                    crosshair.gameObject.SetActive(true);

                    if (t.position.x > 500)
                    {
                        aimCrosshair(t);
                    }
                    else if (t2.position.x > 500)
                    {
                        aimCrosshair(t2);
                    }

                }
              
            }
            
            if (haveWeapon)
            {
                targetVelocityX = (input.x + movementJstick.Horizontal) * (moveSpeed - weaponEquiped.getWeight());
                attackJstickHandle.gameObject.SetActive(false);
                releaseWeaponBtn.gameObject.SetActive(true);
                crosshair.gameObject.SetActive(true);

                if (t.position.x > 500)
                {
                    aimCrosshair(t);
                }
                  
            }
            else
            {
                crosshair.gameObject.SetActive(false);
                targetVelocityX = (input.x + movementJstick.Horizontal) * moveSpeed;
                if (attackJstickHandle.gameObject.activeSelf)
                {
                    PlayerAttack();
                }
            }

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);
        }
           
        
        //movimiento en x 
       
        //gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        

        //Vector3 touch = Input.GetTouch(0).position;
        //print(touch);
        //Camera.main.WorldToScreenPoint(touch);
        
        
        
        
      
    }
    void aimCrosshair(Touch t)
    {
        if ((t.phase.Equals(TouchPhase.Stationary)||(t.phase.Equals(TouchPhase.Moved))))
            {

            crosshair.position = Camera.main.ScreenToWorldPoint(new Vector3(
            t.position.x,
            t.position.y,
            -Camera.main.transform.position.z));


            weaponEquiped.WeaponAttack();
        }
    }
    void UpdateWeaponPosition(Vector3 velocity ){
        float directionX = Mathf.Sign(velocity.x);
        float range = 0.62f;      
        attackCollider.offset = new Vector2(directionX*range,0);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.CompareTag("Weapon"))
        {
            collectWeaponBtn.gameObject.SetActive(true);
            releaseWeaponBtn.gameObject.SetActive(false);
            lastWeaponPased = collision.gameObject.GetComponent<Weapon>();

        }
        
    

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            collectWeaponBtn.gameObject.SetActive(false);          

        }

    }
    private void PlayerAttack()
    {
        attackCollider.enabled = true;
        print("atackpunch");
    }
    public void CollectWeapon()
    {
        haveWeapon = true;        
        weaponEquiped = lastWeaponPased;        
    }
    public void ReleaseWeapon()
    {
        haveWeapon = false;
        releaseWeaponBtn.gameObject.SetActive(false);

    }
      

}

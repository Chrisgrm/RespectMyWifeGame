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
    bool haveWeapon=false;
    public RectTransform asd;
    Weapon lastWeaponPased;


    void Start()
    {
        controller = GetComponent<Controller2D>();
        attackCollider = transform.GetChild(1).GetComponent<CapsuleCollider2D>();

        gravity = -(2 * jumpHigh) / Mathf.Pow(time2JumpMaxHigh, 2);
        jumpVelocity = Mathf.Abs(gravity)  * time2JumpMaxHigh;
      
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponPosition(velocity);
        
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if ((Input.GetButtonDown("Jump")|| movementJstick.Vertical > .75f) && controller.collisionInfo.below && canJump)
        {
            velocity.y = jumpVelocity;
            canJump = false;
        }
        if (movementJstick.Vertical < .2f) canJump = true;

        float targetVelocityX = (input.x + movementJstick.Horizontal) * moveSpeed;      
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirbone);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            if (haveWeapon)
            {
                weaponEquiped.WeaponAttack();
            }
            else
            {
                PlayerAttack();
            }
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
            asd.gameObject.SetActive(true);
            lastWeaponPased = collision.gameObject.GetComponent<Weapon>();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            asd.gameObject.SetActive(false);
            

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
      

}

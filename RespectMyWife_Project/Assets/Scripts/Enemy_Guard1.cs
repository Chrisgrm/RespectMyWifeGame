using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Guard1 : Enemy
{
    //Direccion inicial, true para derecha false para izquierda.
    public bool initialDirection;
    CircleCollider2D attackCollider;
    bool OnVision=false;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        jumpHigh =130;
        time2JumpMaxHigh= .5f;
        moveSpeed = 200;
        accelerationTimeAirbone = .5f;
        accelerationTimeGrounded = .2f;
        enemyNoiseSensibility = 260;
        visionCamp= 750;
        atackRange = 60;
        StartConfig(initialDirection);
        attackCollider.enabled = false;
        


    }

    // Update is called once per frame
    void Update()
    {
        StandardActions();
    }
    public override void AtackAction()
    {
        UpdateWeaponPosition(velocity);
        base.AtackAction();
        velocity.x= 0;
        attackCollider.enabled = true;


    }
    public override void OnVisionEnter(RaycastHit2D hit)
    {
        
        base.OnVisionEnter(hit);
        OnVision = true;  

    }
    void UpdateWeaponPosition(Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float range = 0.62f;
        attackCollider.offset = new Vector2(directionX * range, 0);

    }
    public bool getOnvision()
    {
        return OnVision;
    }
}   

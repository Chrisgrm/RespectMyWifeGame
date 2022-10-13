using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : Enemy
{   
    //Direccion inicial, true para derecha false para izquierda.
    public bool initialDirection;
    public Enemy_Guard1 masterGuard;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        masterGuard = FindObjectOfType<Enemy_Guard1>();
        jumpHigh =130;
        time2JumpMaxHigh= .5f;
        moveSpeed = 600;
        accelerationTimeAirbone = .5f;
        accelerationTimeGrounded = 0.8f;
        enemyNoiseSensibility = 500;
        visionCamp= 1100;
        atackRange = 50;       
        StartConfig(initialDirection);        

    }

    // Update is called once per frame
    void Update()
    {

        StandardActions();


    }
    public override void AtackAction()
    {

        base.AtackAction();   




    }
    public override void OnVisionEnter(RaycastHit2D hit)
    {
        
        if (masterGuard.getOnvision())
        {   
            base.OnVisionEnter(hit);
        }
        
        



    }
}

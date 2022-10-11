using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Guard1 : Enemy
{
    //Direccion inicial, true para derecha false para izquierda.
    public bool initialDirection;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        jumpHigh =130;
        time2JumpMaxHigh= .5f;
        moveSpeed = 200;
        accelerationTimeAirbone = .5f;
        accelerationTimeGrounded = .2f;
        enemyNoiseSensibility = 260;
        visionCamp= 750;
        atackRange = 20;
        StartConfig(initialDirection);
        
    }

    // Update is called once per frame
    void Update()
    {
        StandardActions();
    }
}

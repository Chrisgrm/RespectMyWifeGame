using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : Enemy
{   
    public bool initialDirectionRight;
    // Start is called before the first frame update
    void Start()
    {
        jumpHigh=130;
        time2JumpMaxHigh= .5f;
        moveSpeed = 600;
        accelerationTimeAirbone = .5f;
        accelerationTimeGrounded = 10f;
        enemyNoiseSensibility = 260;
        visionCamp= 750;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        StandardActions();
    }
}

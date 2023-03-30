using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    float weight;
    public Transform FirePoint;
    public GameObject Bullet;
    Vector3 shootPosition;
    void Start()
    {   
        weight=300;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WeaponAttack(Vector3 shootDirection)
    {        
        print("attackckckckc");
        Rigidbody bulletsrb;
        GameObject bullets;
        bullets = Instantiate(Bullet, FirePoint.position, Quaternion.identity);
        bulletsrb= bullets.GetComponent<Rigidbody>();
        bulletsrb.velocity = shootDirection * 1000;


    }
    public float getWeight(){
        return weight;
    }
    public Vector3 getShootPosition()
    {
        return shootPosition;
    }
} 



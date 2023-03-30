using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity=50;
    private Rigidbody Rb;    
    
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
       

    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        
        Destroy(gameObject, 0f);
        
    }
    public void Shoot(Vector3 ShootDirection)
    {
        Rb.velocity = ShootDirection * velocity;
    }
   
   
}

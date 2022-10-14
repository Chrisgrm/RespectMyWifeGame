using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    float weight;
    void Start()
    {   
        weight=300;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WeaponAttack()
    {
        print("attackckckckc");
    }
    public float getWeight(){
        return weight;
    }
}


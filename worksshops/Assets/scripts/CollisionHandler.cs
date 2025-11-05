using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Sphere") && gameObject.CompareTag("Cube"))
        {
            Debug.Log("The asteriod has hit the earth!");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

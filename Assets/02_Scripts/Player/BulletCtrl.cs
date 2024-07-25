using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 1500f;
    public Rigidbody rb;
    public int damage = 20;
    
    void Start()
    {
        rb.AddForce(transform.forward * speed);

        Destroy(gameObject, 3.0f);
    }
}

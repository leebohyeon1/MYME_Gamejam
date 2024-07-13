using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    Rigidbody2D rb;

    public float Speed = 30f;

    public bool goRight = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
        if(goRight)
        {
            rb.velocity = Vector2.right * Speed;
        }
        else
        {
            rb.velocity = Vector2.left * Speed;
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Car);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
       Destroy(gameObject);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buliding : MonoBehaviour
{
    Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0.5f), 0.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.5f);
        }
    }
}

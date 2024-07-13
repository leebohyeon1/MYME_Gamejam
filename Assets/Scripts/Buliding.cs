using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buliding : MonoBehaviour
{
    Transform Player;
    Transform child;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindFirstObjectByType<PlayerController>().transform;
        if(transform.childCount > 0)
        {
            child = transform.GetChild(0);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if(child == null)
        {
            return;
        }

        foreach (GameObject a in GameManager.Instance.ZombieList)
        {
            if (a.transform.position.y < child.position.y)
            {
                a.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder -1;
            }
            else
            {
                a.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }

       if(Player.position.y < child.position.y)
        {
            transform.GetComponent<SpriteRenderer>().sortingOrder = Player.GetComponent<SpriteRenderer>().sortingOrder-1;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sortingOrder = Player.GetComponent<SpriteRenderer>().sortingOrder + 1;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0.5f), 0.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.5f);
        }
    }
}

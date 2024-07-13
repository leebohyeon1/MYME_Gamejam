using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public int maxBox = 3;
    public int curBox;

    private Rigidbody2D rb;
    Vector2 movement;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;

        if(moveHorizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(moveHorizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isMove", true);
        }
       
    }
    public void GetBox()
    {
        curBox++;
        GameManager.Instance.ActivateLocation();
    }

    public void DropBox()
    {
        --curBox;
        Destroy(transform.GetChild(curBox).gameObject);
        GameManager.Instance.RemoveBoxList(transform.GetChild(curBox).gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            EventManager.Instance.PostNotification(EVENT_TYPE.DEAD, this);
            GameManager.Instance.isGameOver = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Delivery"))
        {
            DropBox();
            GameManager.Instance.DeactivateLocation(collision.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            EventManager.Instance.PostNotification(EVENT_TYPE.DEAD, this);
            GameManager.Instance.isGameOver = true;
        }
    }
    public Vector2 GetVector()
    {        
        return movement.normalized;
    }

}

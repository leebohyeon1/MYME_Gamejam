using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public int maxBox = 3;
    public int curBox;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Delivery"))
        {
            DropBox();
            GameManager.Instance.DeactivateLocation(collision.gameObject);
        }
    }
}

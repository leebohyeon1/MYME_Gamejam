using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] BoxParents;
    public float speed = 5f;

    public int maxBox = 3;
    public int curBox;

    private Rigidbody2D rb;
    Vector2 movement;

    Animator animator;

    bool isDead;
    bool isDash;
    float timer = 0;
    public float throwForce = 10f; // 던지는 힘의 크기
    public Vector3 throwDirectionRandomness = new Vector3(1, 1, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isDead = false;
    }

    void Update()
    {
        if(!isDead && !GameManager.Instance.isCount && !isDash)
        {
            Move();
        }
        
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            rb.AddForce(movement* 20, ForceMode2D.Impulse);
            isDash = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
        if(isDash)
        {
            timer += Time.deltaTime;
            if(timer>0.2f)
            {
                isDash = false;
                GetComponent<Collider2D>().isTrigger = false;
                timer = 0;
            }
        }
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
        
        GameManager.Instance.ActivateLocation();
        curBox++;
    }

    public void DropBox()
    {
        curBox--;
        Destroy(BoxParents[curBox].transform.GetChild(0).gameObject);
        GameManager.Instance.RemoveBoxList(BoxParents[curBox].transform.GetChild(0).gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();

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
            Die();
        }

        if (collision.CompareTag("Car"))
        {
            Die();
            animator.SetTrigger("isCar");
            if ((collision.transform.position.x -transform.position.x) > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            transform.Translate(-(collision.transform.position - transform.position) * 2, Space.World);
           
        }
    }
    public Vector2 GetVector()
    {        
        return movement.normalized;
    }

    public void Die()
    {
        if(isDash)
        {
            return;
        }
        rb.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        EventManager.Instance.PostNotification(EVENT_TYPE.DEAD, this);
        GameManager.Instance.isGameOver = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        isDead = true;

        ThrowAllObjects();

    }

    public void DeadForZombie(int i, GameObject Zombie)
    {
        if(i == 0)
        {
            animator.SetTrigger("isRightZombie");
        }
        else if (i == 1)
        {
            animator.SetTrigger("isLeftZombie");
        }
        StartCoroutine(DestroyZom(Zombie));
    }

    public IEnumerator DestroyZom(GameObject Zombie)
    {
        yield return new WaitForSeconds(0.2f);
        DestroyImmediate(Zombie);
    }

    public void ThrowAllObjects()
    {
        for (int i = 0; i < curBox; i++)
        {
            Rigidbody2D rb = BoxParents[i].transform.GetChild(0).GetComponent<Rigidbody2D>();
            BoxParents[i].transform.GetChild(0).transform.parent = null;
            Vector3 throwDirection = CalculateThrowDirection();
          
            rb.AddForce(throwDirection * throwForce,ForceMode2D.Impulse);
        }
    }
    

    Vector3 CalculateThrowDirection()
    {
        float x = Random.Range(-throwDirectionRandomness.x, throwDirectionRandomness.x);
        float y = Random.Range(-throwDirectionRandomness.y, throwDirectionRandomness.y); // 항상 위로 던져지도록
        return new Vector3(x, y, 0).normalized;
    }
}


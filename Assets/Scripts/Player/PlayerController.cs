using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] BoxParents;
    public float speed = 5f;
    public int maxBox = 3;
    public int curBox;
    public float throwForce = 10f;
    public Vector3 throwDirectionRandomness = new Vector3(1, 1, 0);

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDead = false;
    }

    void Update()
    {
        if (!isDead && !GameManager.Instance.isCount)
        {
            Move();
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;

        UpdateSpriteDirection(moveHorizontal);
        UpdateAnimationState(moveHorizontal, moveVertical);
    }

    private void UpdateSpriteDirection(float moveHorizontal)
    {
        GetComponent<SpriteRenderer>().flipX = moveHorizontal > 0;
    }

    private void UpdateAnimationState(float moveHorizontal, float moveVertical)
    {
        animator.SetBool("isMove", moveHorizontal != 0 || moveVertical != 0);
    }

    public void GetBox()
    {
        GameManager.Instance.ActivateLocation();
        curBox++;
    }

    public void DropBox()
    {
        if (curBox <= 0) return;

        GameObject box = BoxParents[curBox - 1].transform.GetChild(0).gameObject;
        Destroy(box);
        GameManager.Instance.RemoveBoxList(box);
        curBox--;
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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GiveBox);
            DropBox();
            GameManager.Instance.DeactivateLocation(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy") || collision.CompareTag("Car") || collision.CompareTag("Explosion"))
        {
            Die();
            HandleCollisionAnimation(collision);
        }
    }

    private void HandleCollisionAnimation(Collider2D collision)
    {
        if (collision.CompareTag("Car") || collision.CompareTag("Explosion"))
        {
            animator.SetTrigger("isCar");
            GetComponent<SpriteRenderer>().flipX = (collision.transform.position.x - transform.position.x) > 0;
            transform.Translate(-(collision.transform.position - transform.position) * 2, Space.World);
        }
    }

    public Vector2 GetVector()
    {
        return movement.normalized;
    }

    public void Die()
    {
        rb.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        EventManager.Instance.PostNotification(EVENT_TYPE.DEAD, this);
        GameManager.Instance.isGameOver = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        isDead = true;

        ThrowAllObjects();
    }

    public void DeadForZombie(int direction, GameObject zombie)
    {
        animator.SetTrigger(direction == 0 ? "isRightZombie" : "isLeftZombie");
        StartCoroutine(DestroyZombie(zombie));
    }

    private IEnumerator DestroyZombie(GameObject zombie)
    {
        yield return new WaitForSeconds(0.2f);
        DestroyImmediate(zombie);
    }

    public void ThrowAllObjects()
    {
        for (int i = 0; i < curBox; i++)
        {
            GameObject box = BoxParents[i].transform.GetChild(0).gameObject;
            Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
            box.transform.parent = null;
            Vector3 throwDirection = CalculateThrowDirection();

            boxRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
        curBox = 0; // Reset curBox after throwing all boxes
    }

    private Vector3 CalculateThrowDirection()
    {
        float x = Random.Range(-throwDirectionRandomness.x, throwDirectionRandomness.x);
        float y = Random.Range(-throwDirectionRandomness.y, throwDirectionRandomness.y);
        return new Vector3(x, y, 0).normalized;
    }
}
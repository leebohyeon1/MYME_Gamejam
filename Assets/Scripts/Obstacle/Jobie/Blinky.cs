using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Blinky : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public bool isBite = false;
    public bool canMove = false;
    private const float MOVE_DELAY = 1f;
    void Start()
    {
        canMove = false;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        if (PlayerPrefs.HasKey("Count"))
        {
            if (PlayerPrefs.GetInt("Count") == 1)
            {
                StartCoroutine(Move(3f));
            }
            else
            {
                StartCoroutine(Move(0.1f));
            }
        }
        else
        {
            StartCoroutine(Move(0.1f));
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    IEnumerator Move(float i)
    {
        yield return new WaitForSeconds(i);
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!canMove)
        {
            return;
        }
        if (!isBite)
        {
            agent.SetDestination(target.transform.position);


            if (target.transform.position.x - transform.position.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBite = true;
            ProcessCollision(collision);
        }
    }

    void ProcessCollision(Collision2D collision)
    {
        float targetOffsetX = (collision.transform.position.x - transform.position.x > 0) ? -1 : 1;
        bool shouldFlip = collision.gameObject.GetComponent<SpriteRenderer>().flipX == false;

        transform.DOMove( new Vector2(collision.transform.position.x +  targetOffsetX, collision.transform.position.y), MOVE_DELAY);

        collision.gameObject.GetComponent<PlayerController>().DeadForZombie(shouldFlip?0:1, gameObject);
    }

   
}

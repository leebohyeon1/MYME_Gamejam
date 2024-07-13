using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pinky : MonoBehaviour
{
    [SerializeField] GameObject target;
    NavMeshAgent agent;
    SpriteRenderer spriteRenderer;

    public float distance = 4f;
    public bool isBite = false;
    public bool canMove = false;
    private const float MOVE_DELAY = 1f;

    void Start()
    {
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
        spriteRenderer = GetComponent<SpriteRenderer>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    IEnumerator Move(float i)
    {
        yield return new WaitForSeconds(i);
        canMove = true;
    }
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (!isBite)
        {
            MoveTowardsPredictedPosition();
        }
    }

    void MoveTowardsPredictedPosition()
    {
        Vector2 predictedTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y) + (target.GetComponent<PlayerController>().GetVector() * distance);
        agent.SetDestination(predictedTargetPosition);

        spriteRenderer.flipX = (predictedTargetPosition.x - transform.position.x > 0);
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

        transform.DOMove(new Vector2(collision.transform.position.x + targetOffsetX, collision.transform.position.y), MOVE_DELAY);

        collision.gameObject.GetComponent<PlayerController>().DeadForZombie(shouldFlip ? 0 : 1, gameObject);
    }

}

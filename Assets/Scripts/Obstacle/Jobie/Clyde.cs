using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Clyde : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public float distance = 8f;

    private Vector3 randomMovePoint;

    private bool isScattering = false;
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
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    IEnumerator Move(float i)
    {
        yield return new WaitForSeconds(i);
        canMove = true;
    }
    private void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (!isBite)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToPlayer > distance && isScattering == false)
            {
                ChasePlayer();
                if (target.transform.position.x - transform.position.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else
            {
                if (!isScattering)
                {
                    FindNewRandomPoint();
                    isScattering = true;
                }
                MoveToRandomPoint();
                if (randomMovePoint.x - transform.position.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
        
    }

    void ChasePlayer()
    {
        agent.SetDestination(target.transform.position);
         // 추적 상태 재설정
    }

    void FindNewRandomPoint()
    {
        float randomX = Random.Range(-50.0f, 50.0f);
        float randomY = Random.Range(-50.0f, 50.0f);
        randomMovePoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomY);
        // 유효한 NavMesh 위치를 찾을 때까지 반복
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(randomMovePoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            randomX = Random.Range(-50.0f, 50.0f);
            randomY = Random.Range(-50.0f, 50.0f);
            randomMovePoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomY);
        }
        randomMovePoint = hit.position;
    }

    void MoveToRandomPoint()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, randomMovePoint);
        agent.SetDestination(randomMovePoint);
        if(distanceToPlayer < 4f)
        {
            isScattering = false;
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

        transform.DOMove(new Vector2(collision.transform.position.x + targetOffsetX, collision.transform.position.y), MOVE_DELAY);

        collision.gameObject.GetComponent<PlayerController>().DeadForZombie(shouldFlip ? 0 : 1, gameObject);
    }

  
}


using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Clyde : MonoBehaviour, IListener
{
    [SerializeField] private GameObject target;

    private NavMeshAgent agent;
    private Vector3 randomMovePoint;

    public float distance = 8f;
    private bool isScattering = false;
    private bool isBite = false;
    private bool canMove = false;
    private const float MOVE_DELAY = 1f;

    private void Start()
    {
        InitializeTarget();
        StartMoveCoroutine();
        InitializeAgent();
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
    }

    private void InitializeTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void StartMoveCoroutine()
    {
        float delay = PlayerPrefs.HasKey("Count") && PlayerPrefs.GetInt("Count") == 1 ? 3f : 0.1f;
        StartCoroutine(MoveAfterDelay(delay));
    }

    private void InitializeAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        if (eventType == EVENT_TYPE.DEAD)
        {
            StopAgentMovement();
        }
    }

    private void StopAgentMovement()
    {
        agent.ResetPath();
        canMove = false;
    }

    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    private void Update()
    {
        if (!canMove || isBite)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToPlayer > distance && !isScattering)
        {
            ChasePlayer();
        }
        else
        {
            if (!isScattering)
            {
                FindNewRandomPoint();
                isScattering = true;
            }
            MoveToRandomPoint();
        }
        UpdateSpriteDirection(distanceToPlayer > distance ? target.transform.position : randomMovePoint);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(target.transform.position);
    }

    private void FindNewRandomPoint()
    {
        randomMovePoint = GenerateRandomNavMeshPoint();
    }

    private Vector3 GenerateRandomNavMeshPoint()
    {
        Vector3 randomPoint;
        NavMeshHit hit;

        do
        {
            float randomX = Random.Range(-50.0f, 50.0f);
            float randomY = Random.Range(-50.0f, 50.0f);
            randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomY);
        }
        while (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas));

        return hit.position;
    }

    private void MoveToRandomPoint()
    {
        agent.SetDestination(randomMovePoint);
        float distanceToPoint = Vector3.Distance(transform.position, randomMovePoint);

        if (distanceToPoint < 4f)
        {
            isScattering = false;
        }
    }

    private void UpdateSpriteDirection(Vector3 targetPosition)
    {
        GetComponent<SpriteRenderer>().flipX = targetPosition.x - transform.position.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleCollisionWithPlayer(collision);
        }
    }

    private void HandleCollisionWithPlayer(Collision2D collision)
    {
        isBite = true;
        ProcessCollision(collision);
    }

    private void ProcessCollision(Collision2D collision)
    {
        float targetOffsetX = collision.transform.position.x - transform.position.x > 0 ? -1 : 1;
        bool shouldFlip = !collision.gameObject.GetComponent<SpriteRenderer>().flipX;

        transform.DOMove(new Vector2(collision.transform.position.x + targetOffsetX, collision.transform.position.y), MOVE_DELAY);
        collision.gameObject.GetComponent<PlayerController>().DeadForZombie(shouldFlip ? 0 : 1, gameObject);
    }
}
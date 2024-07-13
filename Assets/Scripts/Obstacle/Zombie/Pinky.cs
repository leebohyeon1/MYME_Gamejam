using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pinky : MonoBehaviour, IListener
{
    [SerializeField] private GameObject target;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    public float distance = 4f;
    private bool isBite = false;
    private bool canMove = false;
    private const float MOVE_DELAY = 1f;

    private void Start()
    {
        InitializeTarget();
        StartMoveCoroutine();
        InitializeComponents();
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

    private void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        MoveTowardsPredictedPosition();
    }

    private void MoveTowardsPredictedPosition()
    {
        Vector2 predictedTargetPosition = (Vector2)target.transform.position + (target.GetComponent<PlayerController>().GetVector() * distance);
        agent.SetDestination(predictedTargetPosition);
        spriteRenderer.flipX = predictedTargetPosition.x - transform.position.x > 0;
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
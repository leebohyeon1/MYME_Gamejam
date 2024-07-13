using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Inky : MonoBehaviour,IListener
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public float distance = 2f;

    GameObject blinky;
    public bool isBite = false;
    public bool canMove = false;
    private const float MOVE_DELAY = 1f;
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        blinky = FindAnyObjectByType<Blinky>().gameObject;
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
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        agent.ResetPath();
        canMove = false;
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

        if (blinky == null)
        {
            return;
        }
        if (!isBite)
            InkyMove();
    }
    void InkyMove()
    {
        
        Vector2 vector = new Vector2(blinky.transform.position.x, blinky.transform.position.y) - 
            (new Vector2(target.transform.position.x, target.transform.position.y) +
            (target.GetComponent<PlayerController>().GetVector() * distance));

        agent.SetDestination(vector);

        if (vector.x - transform.position.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
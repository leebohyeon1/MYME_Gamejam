using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Inky : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public float distance = 2f;

    GameObject blinky;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        blinky = FindAnyObjectByType<Blinky>().gameObject;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        InkyMove();
    }
    void InkyMove()
    {

        Vector2 vector = new Vector2(blinky.transform.position.x, blinky.transform.position.y) - 
            (new Vector2(target.transform.position.x, target.transform.position.y) +
            (target.GetComponent<PlayerController>().GetVector() * distance));

        agent.SetDestination(vector);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}

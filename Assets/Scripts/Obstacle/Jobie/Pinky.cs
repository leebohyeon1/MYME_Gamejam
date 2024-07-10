using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pinky : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public float distance = 4f;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        PinkyMove();
    }

    void PinkyMove()
    {
        Vector2 Target = new Vector2(target.transform.position.x, target.transform.position.y) + (target.GetComponent<PlayerController>().GetVector() * distance);
        agent.SetDestination(Target);
    }
}

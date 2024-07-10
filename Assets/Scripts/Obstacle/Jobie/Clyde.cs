using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Clyde : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    public float distance = 8f;

    private Vector3 randomMovePoint;

    private bool isScattering = false;

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

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToPlayer > distance && isScattering == false)
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
}


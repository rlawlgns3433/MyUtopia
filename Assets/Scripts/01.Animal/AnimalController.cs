using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AnimalController : MonoBehaviour
{
    public AnimalWork animalWork;
    private NavMeshAgent agent;
    private Node behaviorTreeRoot;
    private bool destinationSet;
    public float range = 10.0f;

    public bool DestinationSet
    {
        get
        {
            return destinationSet;
        }
        set
        {
            destinationSet = value;
        }
    }

    public bool IsEndMovement
    {
        get
        {
            if (agent.pathPending)
                return false;
            if (agent.remainingDistance > agent.stoppingDistance)
                return false;

            if (!agent.hasPath || Mathf.Approximately(agent.velocity.magnitude, 0f) || agent.remainingDistance < agent.stoppingDistance)
            {
                DestinationSet = false;
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        animalWork = GetComponent<AnimalWork>();
        agent = GetComponent<NavMeshAgent>();
        InitializeBehaviorTree();
    }

    private void InitializeBehaviorTree()
    {
        behaviorTreeRoot = new RandomSelector(new List<Node>
        {
            new IdleNode(this),
            new WalkNode(this, RandomDestination, Walk),
            new RunNode(this, RandomDestination, Run),
        });
    }

    private void Update()
    {
        behaviorTreeRoot.Execute();


    }

    public void SetDestination(Vector3 destination)
    {
        if (transform == null)
            return;

        agent.isStopped = false;
        agent.destination = destination;
    }


    public void RandomDestination()
    {
        DestinationSet = true;
        SetDestination(range, out var result);
        SetDestination(result);
    }

    public void Idle()
    {
        agent.isStopped = true;
        agent.speed = 0f;
    }

    public void Walk()
    {
        agent.isStopped = false;
        agent.speed = animalWork.Animal.WalkSpeed;
    }

    public void Run()
    {
        agent.isStopped = false;
        agent.speed = animalWork.Animal.WalkSpeed;
    }

    bool SetDestination(float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 4.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
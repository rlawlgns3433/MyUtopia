using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AnimalController : MonoBehaviour
{
    public Animal animalStat;
    private NavMeshAgent agent;
    private Node behaviorTreeRoot;
    private CancellationTokenSource cts = new CancellationTokenSource();
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
        agent = GetComponent<NavMeshAgent>();
        InitializeBehaviorTree();
    }

    private async void Start()
    {
        await AutoHarvesting();
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
        agent.speed = animalStat.walkSpeed;
    }

    public void Run()
    {
        agent.isStopped = false;
        agent.speed = animalStat.runSpeed;
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

    public async UniTask AutoHarvesting()
    {
        while(true)
        {
            await UniTask.Delay(1000);
        }
    }


    //public void SetDestination(Transform destination)
    //{
    //    if (transform == null)
    //        return;
    //    targetWaypoint = destination;
    //    agent.isStopped = false;
    //    agent.destination = targetWaypoint.position;
    //}

    //public void SetDestination(int index)
    //{
    //    if (waypoints[index] == null)
    //        return;

    //    SetDestination(waypoints[index]);
    //}

    //public void RandomDestination()
    //{
    //    DestinationSet = true;
    //    SetDestination(Random.Range(0, waypoints.Length));
    //}

}
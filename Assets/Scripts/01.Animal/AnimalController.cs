using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public AnimalStat animalStat;
    //public Animator animator;
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private Transform targetWaypoint;
    private Node behaviorTreeRoot;
    private bool destinationSet;

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

    public void SetDestination(Transform destination)
    {
        if (transform == null)
            return;
        targetWaypoint = destination;
        agent.isStopped = false;
        agent.destination = targetWaypoint.position;
    }

    public void SetDestination(int index)
    {
        if (waypoints[index] == null)
            return;

        SetDestination(waypoints[index]);
    }

    public void RandomDestination()
    {
        DestinationSet = true;
        SetDestination(Random.Range(0, waypoints.Length));
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
}
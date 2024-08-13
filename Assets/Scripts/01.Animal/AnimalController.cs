using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum AnimalState
{
    Idle,
    Walk,
    Run,
    Rest,
    Work,
}

public class AnimalController : MonoBehaviour
{
    public AnimalState animalState = AnimalState.Idle;
    public float StateTimer { get; set; }
    public float SetTime { get; set; }

    public AnimalWork animalWork;
    public Animator animator;
    private NavMeshAgent agent;
    public BehaviourSetNode behaviorTreeRoot;
    private bool destinationSet;
    public float range = 10.0f;
    private float timer = 0f;
    private float interval = 7f;
    private Vector3 prevDestination;
    public List<Waypoint> wayPoints = new List<Waypoint>();
    public List<Waypoint> WayPoints
    {
        get
        {
            var floor = FloorManager.Instance.floors[animalWork.Animal.animalStat.CurrentFloor];
            wayPoints = floor.GetComponent<FloorWaypoint>().waypoints;

            return wayPoints;
        }
    }
    public Waypoint CurrentWaypoint { get; set; }

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
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        InitializeBehaviorTree();
    }

    private void InitializeBehaviorTree()
    {
        behaviorTreeRoot = new BehaviourSetNode(this);
    }

    private void Update()
    {
        behaviorTreeRoot.Execute();
        StateTimer += Time.deltaTime;
    }

    public void SetDestination(Vector3 destination)
    {
        if (transform == null)
            return;

        agent.isStopped = false;
        agent.destination = destination;
        prevDestination = agent.destination;
    }


    public void RandomDestination()
    {
        DestinationSet = true;
        SetDestination(range, out var result);
        SetDestination(result);
    }

    public void Rest()
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
        agent.speed = animalWork.Animal.RunSpeed;
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

    public bool EndTimer()
    {
        return StateTimer >= SetTime;
    }

    public void SetDestination(Waypoint waypoint)
    {
        agent.isStopped = false;
        agent.destination = waypoint.transform.position;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSetNode : StandardNode
{
    public bool IsSetBehaviour { get; set; }

    protected RandomSelector behaviourSetSubTree; // left
    protected Selector behaviourSubTree; // right
    public BehaviourSetNode() { }
    public BehaviourSetNode(AnimalController animalController) : base(animalController) 
    {
        InitializeBehaviorTree();
    }

    public BehaviourSetNode(AnimalController animalController, params Action[] actions)
    {
        base.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public void InitializeBehaviorTree()
    {

        #region EmptyBehaviour
        List<Node> behaviourSet = new List<Node>();

        // 행동이 없을 때 좌측 서브 트리
        var randomLeft = new List<Node>
        {
            new WalkNode(animalController),
            new RestNode(animalController)
        };

        //행동이 없을 때 우측 서브 트리 => waypoint 추가하고 주석 해제
        var randomRight = new List<Node>();

        foreach (var wayPoint in animalController.WayPoints)
        {
            var wayPointNode = new WayPointNode(animalController, wayPoint);
            randomRight.Add(wayPointNode);
        }

        var randomSelectorLeft = new RandomSelector(randomLeft);
        var randomSelectorRight = new RandomSelector(randomRight);

        behaviourSet.Add(randomSelectorLeft);
        behaviourSet.Add(randomSelectorRight);

        behaviourSetSubTree = new RandomSelector(behaviourSet);

        #endregion EmptyBehaviour


        #region Behaviour

        List<Node> behaviours = new List<Node>();

        List<Node> selectorBehaviour = new List<Node>();

        selectorBehaviour.Add(new WalkBehaviourNode(animalController, animalController.RandomDestination, animalController.Walk));
        selectorBehaviour.Add(new RestBehaviourNode(animalController, animalController.Rest));

        List<Node> workSequence = new List<Node>();
        workSequence.Add(new RunBehaviourNode(animalController, animalController.Run));
        workSequence.Add(new WorkBehaviourNode(animalController));

        var workBehaviourNode = new Sequence(workSequence);
        
        selectorBehaviour.Add(workBehaviourNode);

        var selector = new Selector(selectorBehaviour);
        behaviours.Add(selector);

        behaviourSubTree = new Selector(behaviours);
        #endregion Behaviour
    }

    public override bool Execute()
    {
        if(IsSetBehaviour)
        {
            return behaviourSubTree.Execute();
        }

        IsSetBehaviour = true;
        return behaviourSetSubTree.Execute();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
public class WorkBehaviourNode : Sequence
{
    public WorkBehaviourNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public WorkBehaviourNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public WorkBehaviourNode(AnimalController animalController, List<Node> nodes)
    {
        this.animalController = animalController;
        this.nodes = nodes;
    }

    public void AddActions(params Action[] actions)
    {
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    private void EnterNode()
    {
        animalController.StateTimer = 0;
        animalController.SetTime = 5;
        animalController.animator.Play(AnimationHash.Walk);
    }

    private void ExitNode()
    {
        animalController.SetTime = 0;
        animalController.StateTimer = 0;
        animalController.behaviorTreeRoot.IsSetBehaviour = false;
    }

    public override bool Execute()
    {
        if (animalController.animalState != AnimalState.Work)
            return false;

        if(animalController.SetTime == 0)
        {
            EnterNode();
        }

        if (animalController.EndTimer())
        {
            ExitNode();
            return true;
        }

        // 실제 행동에 대한 코드
        Debug.Log("Working");

        action?.Invoke();

        return animalController.EndTimer();
    }
}
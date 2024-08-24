using System;
using UnityEngine;
public class WalkBehaviourNode : StandardNode
{
    public WalkBehaviourNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public WalkBehaviourNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
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
        animalController.DestinationSet = false;
        animalController.BehaviorTreeRoot.IsSetBehaviour = false;
    }

    public override bool Execute()
    {
        if (animalController.animalState != AnimalState.Walk)
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

        if (!animalController.DestinationSet || animalController.IsEndMovement)
        {
            action?.Invoke();
        }

        return false;
    }

}
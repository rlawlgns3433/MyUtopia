using System;
using UnityEngine;
public class RunBehaviourNode : StandardNode
{
    public RunBehaviourNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public RunBehaviourNode(AnimalController animalController, params Action[] actions)
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
        animalController.SetTime = 10;
        animalController.StateTimer = 0;
        animalController.animator.Play(AnimationHash.Run);
    }

    private void ExitNode()
    {
        animalController.SetTime = 0;
        animalController.StateTimer = 0;
    }

    public override bool Execute()
    {
        if(animalController.IsEndMovement)
        {
            animalController.animalState = AnimalState.Work;
            return true;
        }

        if (animalController.animalState != AnimalState.Run)
            return false;

        if (animalController.SetTime == 0)
        {
            EnterNode();
        }

        if (animalController.EndTimer()) // 도착하면
        {
            ExitNode();
            return true;
        }

        // 실제 행동에 대한 코드

        if (!animalController.DestinationSet)
            action?.Invoke();

        if (animalController.IsEndMovement)
        {
            ExitNode();
        }

        return animalController.IsEndMovement;
    }
}
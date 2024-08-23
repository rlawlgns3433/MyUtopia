using System;
using UnityEngine;
public class RestBehaviourNode : StandardNode
{
    public RestBehaviourNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public RestBehaviourNode(AnimalController animalController, params Action[] actions)
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
        animalController.SetTime = 5;
        animalController.StateTimer = 0;
        animalController.animator.Play(AnimationHash.Rest);
    }

    private void ExitNode()
    {
        animalController.SetTime = 0;
        animalController.StateTimer = 0;
        animalController.behaviorTreeRoot.IsSetBehaviour = false;
    }

    public override bool Execute()
    {
        if (animalController.animalState != AnimalState.Rest)
            return false;

        if(animalController.SetTime == 0)
            EnterNode();

        if (animalController.EndTimer())
            ExitNode();

        // 실제 행동에 대한 코드
        action?.Invoke();

        return true;
    }
}
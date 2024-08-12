using System;
using UnityEngine;

public class RunNode : StandardNode
{
    public RunNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public RunNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public override bool Execute()
    {
        animalController.animalState = AnimalState.Run;
        animalController.behaviorTreeRoot.IsSetBehaviour = true;
        Debug.Log("Run");
        return true;
    }
}
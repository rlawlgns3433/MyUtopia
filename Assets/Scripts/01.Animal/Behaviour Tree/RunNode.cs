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
        animalController.animator.Play(AnimationHash.Walk, animalController.animator.GetLayerIndex("Base Layer"));
        animalController.animator.Play(AnimationHash.eyeAnnoyed, animalController.animator.GetLayerIndex("Shapekey"));
        Debug.Log("Run");
        return true;
    }
}
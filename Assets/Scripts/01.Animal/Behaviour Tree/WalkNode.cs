using System;
using UnityEngine;
public class WalkNode : StandardNode
{
    public WalkNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public WalkNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach(var action in actions)
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

    public override bool Execute()
    {
        animalController.animalState = AnimalState.Walk;
        animalController.animator.Play(AnimationHash.Walk, animalController.animator.GetLayerIndex("Base Layer"));
        animalController.animator.Play(AnimationHash.eyeAnnoyed, animalController.animator.GetLayerIndex("Shapekey"));
        animalController.behaviorTreeRoot.IsSetBehaviour = true;

        Debug.Log("Walk");

        return true;
    }
}
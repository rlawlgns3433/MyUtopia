using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkNode : StandardNode
{
    public WorkNode() { }
    public WorkNode(AnimalController animalController) : base(animalController)
    {
        this.animalController = animalController;
    }

    public WorkNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public override bool Execute()
    {
        animalController.animalState = AnimalState.Work;
        animalController.behaviorTreeRoot.IsSetBehaviour = true;

        Debug.Log("Work");

        return true;
    }
}

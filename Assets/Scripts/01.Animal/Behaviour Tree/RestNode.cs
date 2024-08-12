using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestNode : StandardNode
{
    public RestNode() { }
    public RestNode(AnimalController animalController) : base(animalController)
    {
        this.animalController = animalController;
    }

    public RestNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public override bool Execute()
    {
        animalController.animalState = AnimalState.Rest;
        animalController.behaviorTreeRoot.IsSetBehaviour = true;
        Debug.Log("Rest");

        return true;
    }
}

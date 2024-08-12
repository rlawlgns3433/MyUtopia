using System;
using System.Collections.Generic;

public abstract class StandardNode : Node
{
    protected AnimalController animalController;
    public StandardNode() { }
    public StandardNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public StandardNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }
}

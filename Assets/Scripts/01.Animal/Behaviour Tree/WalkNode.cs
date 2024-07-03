using System;

public class WalkNode : Node
{
    private AnimalController animalController;

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
        if (!animalController.DestinationSet)
        {
            //animalController.RandomDestination();
            //animalController.Walk();
            action?.Invoke();
        }
        return animalController.IsEndMovement;
    }
}
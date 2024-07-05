using System;

public class RunNode : Node
{
    private AnimalController animalController;

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
        if(!animalController.DestinationSet)
        {
            //animalController.RandomDestination();
            //animalController.Run();
            action?.Invoke();
        }

        return animalController.IsEndMovement;
    }
}
using System;

public class ConditionNode : StandardNode
{
    public Func<bool> condition;
    public AnimalState animalState;
    public Node nextNode;
    public ConditionNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public ConditionNode(AnimalController animalController, Func<bool> action)
    {
        this.animalController = animalController;
        condition = action;
    }    
    
    public ConditionNode(AnimalController animalController, AnimalState animalState)
    {
        this.animalController = animalController;
        this.animalState = animalState;
    }    
    
    public ConditionNode(AnimalController animalController, AnimalState animalState, Node nextNode)
    {
        this.animalController = animalController;
        this.animalState = animalState;
        this.nextNode = nextNode;
    }

    public ConditionNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public void SetCondition(Func<bool> action)
    {
        condition = action;
    }

    public void SetNextNode(Node nextNode)
    {
        this.nextNode = nextNode;
    }

    public override bool Execute()
    {
        return nextNode.Execute();
    }
}
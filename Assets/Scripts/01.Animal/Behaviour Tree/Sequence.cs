using System.Collections.Generic;

public class Sequence : StandardNode
{
    protected List<Node> nodes;
    public Sequence() { }
    public Sequence(AnimalController animalController, List<Node> nodes) 
    { 
        this.nodes = nodes;
        this.animalController = animalController;
    }
    public Sequence(List<Node> nodes) { this.nodes = nodes; }

    public override bool Execute()
    {
        foreach (Node node in nodes)
        {
            if (!node.Execute()) return false;
        }
        return true;
    }
}
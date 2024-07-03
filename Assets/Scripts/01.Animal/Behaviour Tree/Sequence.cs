using System.Collections.Generic;

public class Sequence : Node
{
    private List<Node> nodes;
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
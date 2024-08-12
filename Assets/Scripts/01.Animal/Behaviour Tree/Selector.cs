using System.Collections.Generic;

// Selector와 RandomSelect 상속구조로 만들기

public class Selector : Node
{
    private List<Node> nodes;
    public Selector(List<Node> nodes) { this.nodes = nodes; }

    public override bool Execute()
    {
        foreach (Node node in nodes)
        {
            if (node.Execute()) return true;
        }
        return false;
    }
}


public class RandomSelector : Node
{
    private List<Node> nodes;
    private int index = -1;

    public RandomSelector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Execute()
    {
        if (nodes.Count == 0)
        {
            return false;
        }

        if(index == -1)
        {
            index = UnityEngine.Random.Range(0, nodes.Count);
        }

        if (nodes[index].Execute())
        {
            index = -1;
            return true;
        }

        return false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes;
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

public class RandomSelector : Selector
{
    public RandomSelector(List<Node> nodes) : base(nodes)
    {
    }

    public void Shuffle(List<Node> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Node value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public override bool Execute()
    {
        Shuffle(nodes);

        foreach(Node node in nodes)
        {
            if (node.Execute()) return true;
        }
        return false;
    }
}
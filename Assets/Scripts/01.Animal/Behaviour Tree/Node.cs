using System;
using UnityEngine.Events;

public abstract class Node
{
    public Action action;
    public abstract bool Execute();
}
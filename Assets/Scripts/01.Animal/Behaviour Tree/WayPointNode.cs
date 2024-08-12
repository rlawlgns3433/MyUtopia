using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNode : StandardNode
{
    public GameObject wayPoint; // Emtpy 조건 추가를 위해 상호작용하는 오브젝트에 스크립트 필요
    public Node nextNode;
    public WayPointNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }   
    
    public WayPointNode(AnimalController animalController, GameObject wayPoint)
    {
        this.animalController = animalController;
    }

    public WayPointNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public void SetNextNode(Node nextNode)
    {
        this.nextNode = nextNode;
    }

    public override bool Execute()
    {
        Debug.Log("WayPointNode");

        return nextNode.Execute(); // Empty 조건 추가
    }
}

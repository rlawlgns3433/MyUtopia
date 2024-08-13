using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNode : StandardNode
{
    public Waypoint wayPoint; // Emtpy 조건 추가를 위해 상호작용하는 오브젝트에 스크립트 필요
    public Node nextNode;
    public WayPointNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }   
    
    public WayPointNode(AnimalController animalController, Waypoint wayPoint)
    {
        this.animalController = animalController;
        this.wayPoint = wayPoint;
    }

    public WayPointNode(AnimalController animalController, Waypoint wayPoint, params Action[] actions)
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
        if (!wayPoint.IsFull)
            wayPoint.EnterAnimal(animalController.animalWork.Animal.animalStat);
        else
            return false;

        animalController.CurrentWaypoint = wayPoint;
        animalController.animalState = AnimalState.Run;
        animalController.behaviorTreeRoot.IsSetBehaviour = true;
        animalController.SetDestination(wayPoint);
        Debug.Log("WayPointNode");

        return true;
    }
}

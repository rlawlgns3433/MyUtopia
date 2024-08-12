using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNode : StandardNode
{
    public GameObject wayPoint; // Emtpy ���� �߰��� ���� ��ȣ�ۿ��ϴ� ������Ʈ�� ��ũ��Ʈ �ʿ�
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

        return nextNode.Execute(); // Empty ���� �߰�
    }
}

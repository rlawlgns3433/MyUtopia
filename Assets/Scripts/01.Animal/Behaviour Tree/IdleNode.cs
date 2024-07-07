using System;
using UnityEngine;

public class IdleNode : Node
{
    private AnimalController animalController;
    private float delayTime;
    private float timer;

    public IdleNode(AnimalController animalController)
    {
        this.animalController = animalController;
        delayTime = animalController.animalWork.Animal.IdleTime;
        timer = 0f;
    }

    public IdleNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        delayTime = animalController.animalWork.Animal.IdleTime;
        timer = 0f;

        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public override bool Execute()
    {
        timer += Time.deltaTime;

        if (timer > delayTime)
        {
            timer = 0f;
            return true;
        }
        return false;
    }
}
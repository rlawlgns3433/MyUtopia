using System;
using Unity.VisualScripting;
using UnityEngine;
public class WorkBehaviourNode : StandardNode
{
    private bool first = true;
    public WorkBehaviourNode(AnimalController animalController)
    {
        this.animalController = animalController;
    }

    public WorkBehaviourNode(AnimalController animalController, params Action[] actions)
    {
        this.animalController = animalController;
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    public void AddActions(params Action[] actions)
    {
        foreach (var action in actions)
        {
            this.action += action;
        }
    }

    private void EnterNode()
    {
        animalController.StateTimer = 0;
        animalController.SetTime = 5;

        if(animalController.CurrentWaypoint == null)
        {
            ExitNode();
            return;
        }

        float speed = 0f;

        if(animalController.CurrentWaypoint.GetRandomClip(out speed) == -1)
        {
            ExitNode();
            return;
        }

        animalController.animator.Play(animalController.CurrentWaypoint.GetRandomClip(out speed), animalController.animator.GetLayerIndex("Base Layer"));
        animalController.animator.speed = speed;
        animalController.animator.Play(animalController.CurrentWaypoint.GetEyeClip(), animalController.animator.GetLayerIndex("Shapekey"));
        first = false;
    }

    private void ExitNode()
    {
        animalController.SetTime = 0;
        animalController.StateTimer = 0;
        animalController.behaviorTreeRoot.IsSetBehaviour = false;
        first = true;

        if (animalController.CurrentWaypoint == null)
            return;

        animalController.CurrentWaypoint.ExitAnimal(animalController.animalWork.Animal.animalStat);
    }

    public override bool Execute()
    {
        if (!animalController.IsEndMovement)
            return false;

        if (animalController.animalState != AnimalState.Work)
            return false;

        if(first)
        {
            EnterNode();
        }

        if (animalController.EndTimer())
        {
            ExitNode();
            return true;
        }

        // 실제 행동에 대한 코드

        action?.Invoke();

        return animalController.EndTimer();
    }
}
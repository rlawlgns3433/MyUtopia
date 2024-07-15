using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Subject : MonoBehaviour
{
    private readonly List<Observer> observers = new List<Observer>();

    protected void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    protected void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    protected void NotifyObservers()
    {
        foreach (Observer observer in observers)
        {
            observer.Notify(this);
        }
    }
}
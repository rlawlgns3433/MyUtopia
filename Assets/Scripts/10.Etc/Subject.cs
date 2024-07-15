using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Subject : MonoBehaviour
{
    private readonly List<Observer> observers = new List<Observer>();

    protected void Attach(Observer observer)
    {
        if(!observers.Contains(observer))
            observers.Add(observer);
    }

    protected void Detach(Observer observer)
    {
        if(observers.Contains(observer))
            observers.Remove(observer);
    }

    protected void NotifyObservers()
    {
        foreach (Observer observer in observers)
        {
            if (observer == null)
                continue;

            observer.Notify(this);
        }
    }
}
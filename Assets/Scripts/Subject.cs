using UnityEngine;
using System.Collections;

public abstract class Subject : MonoBehaviour
{
    private readonly ArrayList observers = new ArrayList();

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
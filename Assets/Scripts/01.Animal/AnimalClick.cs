using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalClick : MonoBehaviour, IClickable
{
    [SerializeField]
    private bool isFocused = false;
    public bool IsFocused
    {
        get
        {
            return isFocused;
        }

        set
        {
            isFocused = value;
            if (isFocused)
            {
                clickEvent?.Invoke();
            }
        }
    }
    public event Action clickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        IsFocused = true;
    }
}

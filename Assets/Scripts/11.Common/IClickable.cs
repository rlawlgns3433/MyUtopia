using System;
using UnityEngine.EventSystems;

public interface IClickable : IPointerClickHandler
{
    public bool IsFocused { get; set; }
    public event Action clickEvent;
}
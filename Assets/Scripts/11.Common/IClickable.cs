using System;
using UnityEngine.EventSystems;

public interface IClickable : IPointerClickHandler
{
    public bool IsClicked { get; set; }
    public event Action clickEvent;
    public void RegisterClickable();
}
using System;

public interface IClickable
{
    public bool IsFocused { get; set; }
    public event Action clickEvent;
}